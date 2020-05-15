using System;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Results
{
    public class StartTicketResult : IProcessResult<TicketStartResult>, IWorkflowResult
    {
        public StartTicketResult(TicketStartResult result)
        {
            Result = result;
        }

        public TicketStartResult Result { get; }

        public string Message => GetMessage();

        public int TicketId { get; internal set; }

        public int? UserId { get; internal set; }

        public DateTimeOffset? StartedOn { get; internal set; }

        public int? ResolvedBy { get; internal set; }

        public DateTimeOffset? ResolvedOn { get; internal set; }

        public int? ClosedBy { get; internal set; }

        public DateTimeOffset? ClosedOn { get; internal set; }

        public IWorkflowProcess Workflow { get; internal set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketStartResult.Started => ResultMessages.Started,
            TicketStartResult.TicketNotFound => ResultMessages.TicketNotFound,
            TicketStartResult.TicketAlreadyResolved => ResultMessages.TicketAlreadyResolved,
            TicketStartResult.TicketAlreadyClosed => ResultMessages.TicketAlreadyClosed,
            TicketStartResult.TicketAlreadyStarted => ResultMessages.TicketAlreadyStarted,
            TicketStartResult.WorkflowFailed => ResultMessages.WorkflowFailed,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}