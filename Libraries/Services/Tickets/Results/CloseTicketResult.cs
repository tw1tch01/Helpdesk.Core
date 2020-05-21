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
        public int TicketId { get; internal set; }
        public Guid? UserGuid { get; internal set; }
        public DateTimeOffset? ResolvedOn { get; internal set; }
        public Guid? ResolvedBy { get; internal set; }
        public DateTimeOffset? ClosedOn { get; internal set; }
        public Guid? ClosedBy { get; internal set; }
        public IWorkflowProcess Workflow { get; internal set; }

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