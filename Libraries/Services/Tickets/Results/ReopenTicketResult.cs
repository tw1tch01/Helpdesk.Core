using System;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Results
{
    public class ReopenTicketResult : IProcessResult<TicketReopenResult>, IWorkflowResult
    {
        public ReopenTicketResult(TicketReopenResult result)
        {
            Result = result;
        }

        public TicketReopenResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; internal set; }
        public Guid? UserGuid { get; internal set; }
        public IWorkflowProcess Workflow { get; internal set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketReopenResult.Reopened => ResultMessages.Reopened,
            TicketReopenResult.TicketNotFound => ResultMessages.TicketNotFound,
            TicketReopenResult.WorkflowFailed => ResultMessages.WorkflowFailed,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}