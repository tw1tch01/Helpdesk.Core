using System;
using Helpdesk.Domain.Entities;
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

        public int TicketId { get; private set; }
        public DateTimeOffset? ResolvedOn { get; private set; }
        public int? ResolvedBy { get; private set; }
        public int? ClosedBy { get; private set; }
        public DateTimeOffset? ClosedOn { get; private set; }
        public DateTimeOffset? PausedOn { get; private set; }

        public IWorkflowProcess Workflow { get; private set; }

        internal static PauseTicketResult TicketNotFound(int ticketId)
        {
            return new PauseTicketResult(TicketPauseResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        internal static PauseTicketResult TicketAlreadyResolved(Ticket ticket)
        {
            return new PauseTicketResult(TicketPauseResult.TicketAlreadyResolved)
            {
                TicketId = ticket.TicketId,
                ResolvedBy = ticket.ResolvedBy.Value,
                ResolvedOn = ticket.ResolvedOn.Value,
            };
        }

        internal static PauseTicketResult TicketAlreadyClosed(Ticket ticket)
        {
            return new PauseTicketResult(TicketPauseResult.TicketAlreadyClosed)
            {
                TicketId = ticket.TicketId,
                ClosedBy = ticket.ClosedBy.Value,
                ClosedOn = ticket.ClosedOn.Value,
            };
        }

        internal static PauseTicketResult TicketAlreadyPaused(Ticket ticket)
        {
            return new PauseTicketResult(TicketPauseResult.TicketAlreadyPaused)
            {
                TicketId = ticket.TicketId,
                PausedOn = ticket.PausedOn.Value
            };
        }

        internal static PauseTicketResult WorkflowFailed(int ticketId, IWorkflowProcess workflow)
        {
            return new PauseTicketResult(TicketPauseResult.WorkflowFailed)
            {
                TicketId = ticketId,
                Workflow = workflow
            };
        }

        internal static PauseTicketResult Paused(Ticket ticket)
        {
            return new PauseTicketResult(TicketPauseResult.Paused)
            {
                TicketId = ticket.TicketId,
                PausedOn = ticket.PausedOn.Value
            };
        }

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
    }
}