using System;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Results
{
    public class PauseTicketResult : IProcessResult<TicketPauseResult>, IWorkflowResult
    {
        public PauseTicketResult(TicketPauseResult result)
        {
            Result = result;
        }

        public TicketPauseResult Result { get; }

        public string Message => GetMessage();

        public int TicketId { get; internal set; }

        public int? UserId { get; internal set; }

        public DateTimeOffset? ResolvedOn { get; internal set; }

        public int? ResolvedBy { get; internal set; }

        public int? ClosedBy { get; internal set; }

        public DateTimeOffset? ClosedOn { get; internal set; }

        public DateTimeOffset? PausedOn { get; internal set; }

        public IWorkflowProcess Workflow { get; internal set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketPauseResult.Paused => ResultMessages.Paused,
            TicketPauseResult.TicketNotFound => ResultMessages.TicketNotFound,
            TicketPauseResult.TicketAlreadyResolved => ResultMessages.TicketAlreadyResolved,
            TicketPauseResult.TicketAlreadyClosed => ResultMessages.TicketAlreadyClosed,
            TicketPauseResult.TicketAlreadyPaused => ResultMessages.TicketAlreadyPaused,
            TicketPauseResult.WorkflowFailed => ResultMessages.WorkflowFailed,
            _ => Result.ToString(),
        };

        #endregion Methods
    }
}