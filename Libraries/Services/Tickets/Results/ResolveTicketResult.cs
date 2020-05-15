using System;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Results
{
    public class ResolveTicketResult : IProcessResult<TicketResolveResult>, IWorkflowResult
    {
        public ResolveTicketResult(TicketResolveResult result)
        {
            Result = result;
        }

        public TicketResolveResult Result { get; }

        public string Message => GetMessage();

        public int TicketId { get; internal set; }

        public int? UserId { get; internal set; }

        public int? ResolvedBy { get; internal set; }

        public DateTimeOffset? ResolvedOn { get; internal set; }

        public int? ClosedBy { get; internal set; }

        public DateTimeOffset? ClosedOn { get; internal set; }

        public IWorkflowProcess Workflow { get; internal set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketResolveResult.Resolved => ResultMessages.Resolved,
            TicketResolveResult.TicketNotFound => ResultMessages.TicketNotFound,
            TicketResolveResult.TicketAlreadyResolved => ResultMessages.TicketAlreadyResolved,
            TicketResolveResult.TicketAlreadyClosed => ResultMessages.TicketAlreadyClosed,
            TicketResolveResult.UserNotFound => ResultMessages.UserNotFound,
            TicketResolveResult.WorkflowFailed => ResultMessages.WorkflowFailed,
            _ => Result.ToString(),
        };

        #endregion Methods
    }
}