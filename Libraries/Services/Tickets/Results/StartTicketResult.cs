using System;
using Helpdesk.Domain.Entities;
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
        public int TicketId { get; private set; }
        public DateTimeOffset? StartedOn { get; private set; }
        public int? ResolvedBy { get; private set; }
        public DateTimeOffset? ResolvedOn { get; private set; }
        public int? ClosedBy { get; private set; }
        public DateTimeOffset? ClosedOn { get; private set; }
        public IWorkflowProcess Workflow { get; private set; }

        #region Methods

        internal static StartTicketResult Started(Ticket ticket)
        {
            return new StartTicketResult(TicketStartResult.Started)
            {
                TicketId = ticket.TicketId,
                StartedOn = ticket.StartedOn
            };
        }

        internal static StartTicketResult TicketNotFound(int ticketId)
        {
            return new StartTicketResult(TicketStartResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        internal static StartTicketResult TicketAlreadyResolved(Ticket ticket)
        {
            return new StartTicketResult(TicketStartResult.TicketAlreadyResolved)
            {
                TicketId = ticket.TicketId,
                ResolvedBy = ticket.ResolvedBy.Value,
                ResolvedOn = ticket.ResolvedOn.Value
            };
        }

        internal static StartTicketResult TicketAlreadyClosed(Ticket ticket)
        {
            return new StartTicketResult(TicketStartResult.TicketAlreadyClosed)
            {
                TicketId = ticket.TicketId,
                ClosedBy = ticket.ClosedBy.Value,
                ClosedOn = ticket.ClosedOn.Value
            };
        }

        internal static StartTicketResult TicketAlreadyStarted(Ticket ticket)
        {
            return new StartTicketResult(TicketStartResult.TicketAlreadyStarted)
            {
                TicketId = ticket.TicketId,
                StartedOn = ticket.StartedOn.Value,
            };
        }

        internal static StartTicketResult WorkflowFailed(int ticketId, IWorkflowProcess workflow)
        {
            return new StartTicketResult(TicketStartResult.WorkflowFailed)
            {
                TicketId = ticketId,
                Workflow = workflow
            };
        }

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