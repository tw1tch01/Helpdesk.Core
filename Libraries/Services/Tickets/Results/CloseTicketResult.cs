using System;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Results
{
    public class CloseTicketResult : IProcessResult<TicketCloseResult>, IWorkflowResult
    {
        public CloseTicketResult(TicketCloseResult result)
        {
            Result = result;
        }

        public TicketCloseResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; set; }
        public Guid? UserGuid { get; set; }
        public DateTimeOffset? ResolvedOn { get; set; }
        public Guid? ResolvedBy { get; set; }
        public DateTimeOffset? ClosedOn { get; set; }
        public Guid? ClosedBy { get; set; }
        public IWorkflowProcess Workflow { get; set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketCloseResult.Closed => ResultMessages.Closed,
            TicketCloseResult.TicketNotFound => ResultMessages.TicketNotFound,
            TicketCloseResult.TicketAlreadyResolved => ResultMessages.TicketAlreadyResolved,
            TicketCloseResult.TicketAlreadyClosed => ResultMessages.TicketAlreadyClosed,
            TicketCloseResult.WorkflowFailed => ResultMessages.WorkflowFailed,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}