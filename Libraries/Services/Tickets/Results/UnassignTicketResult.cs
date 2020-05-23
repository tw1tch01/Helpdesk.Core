using System;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Results
{
    public class UnassignTicketResult : IProcessResult<TicketUnassignResult>, IWorkflowResult
    {
        public UnassignTicketResult(TicketUnassignResult result)
        {
            Result = result;
        }

        public TicketUnassignResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; set; }
        public Guid? UnassignedBy { get; set; }
        public IWorkflowProcess Workflow { get; set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketUnassignResult.Unassigned => ResultMessages.Unassigned,
            TicketUnassignResult.TicketNotFound => ResultMessages.TicketNotFound,
            TicketUnassignResult.WorkflowFailed => ResultMessages.WorkflowFailed,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}