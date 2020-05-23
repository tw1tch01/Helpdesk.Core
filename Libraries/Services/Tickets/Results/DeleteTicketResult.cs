using System;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Results
{
    public class DeleteTicketResult : IProcessResult<TicketDeleteResult>, IWorkflowResult
    {
        public DeleteTicketResult(TicketDeleteResult result)
        {
            Result = result;
        }

        public TicketDeleteResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; set; }
        public Guid? UserGuid { get; set; }
        public IWorkflowProcess Workflow { get; set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketDeleteResult.Deleted => ResultMessages.Deleted,
            TicketDeleteResult.TicketNotFound => ResultMessages.TicketNotFound,
            TicketDeleteResult.WorkflowFailed => ResultMessages.WorkflowFailed,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}