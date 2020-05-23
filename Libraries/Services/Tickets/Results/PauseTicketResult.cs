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
        public int TicketId { get; set; }
        public Guid? UserGuid { get; set; }
        public DateTimeOffset? ResolvedOn { get; set; }
        public Guid? ResolvedBy { get; set; }
        public DateTimeOffset? ClosedOn { get; set; }
        public Guid? ClosedBy { get; set; }
        public DateTimeOffset? PausedOn { get; set; }
        public Guid? PausedBy { get; set; }
        public IWorkflowProcess Workflow { get; set; }

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