using System;
using Helpdesk.Domain.Entities;
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
        public int TicketId { get; private set; }
        public int? ResolvedBy { get; private set; }
        public DateTimeOffset? ResolvedOn { get; private set; }
        public int? ClosedBy { get; private set; }
        public DateTimeOffset? ClosedOn { get; private set; }
        public int? UserId { get; private set; }
        public IWorkflowProcess Workflow { get; private set; }

        #region Methods

        internal static ResolveTicketResult TicketNotFound(int ticketId)
        {
            return new ResolveTicketResult(TicketResolveResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        internal static ResolveTicketResult TicketAlreadyResolved(Ticket ticket)
        {
            return new ResolveTicketResult(TicketResolveResult.TicketAlreadyResolved)
            {
                TicketId = ticket.TicketId,
                ResolvedBy = ticket.ResolvedBy.Value,
                ResolvedOn = ticket.ResolvedOn.Value
            };
        }

        internal static ResolveTicketResult TicketAlreadyClosed(Ticket ticket)
        {
            return new ResolveTicketResult(TicketResolveResult.TicketAlreadyClosed)
            {
                TicketId = ticket.TicketId,
                ClosedBy = ticket.ClosedBy.Value,
                ClosedOn = ticket.ClosedOn.Value
            };
        }

        internal static ResolveTicketResult UserNotFound(int ticketId, int userId)
        {
            return new ResolveTicketResult(TicketResolveResult.UserNotFound)
            {
                TicketId = ticketId,
                UserId = userId
            };
        }

        internal static ResolveTicketResult WorkflowFailed(int ticketId, IWorkflowProcess workflow)
        {
            return new ResolveTicketResult(TicketResolveResult.WorkflowFailed)
            {
                TicketId = ticketId,
                Workflow = workflow
            };
        }

        internal static ResolveTicketResult Resolved(Ticket ticket)
        {
            return new ResolveTicketResult(TicketResolveResult.Resolved)
            {
                TicketId = ticket.TicketId,
                ResolvedBy = ticket.ResolvedBy.Value,
                ResolvedOn = ticket.ResolvedOn.Value,
                UserId = ticket.ResolvedBy.Value
            };
        }

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