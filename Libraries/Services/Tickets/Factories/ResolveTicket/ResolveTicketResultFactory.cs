using System;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.ResolveTicket
{
    public class ResolveTicketResultFactory : IResolveTicketResultFactory
    {
        public ResolveTicketResult Resolved(Ticket ticket)
        {
            return new ResolveTicketResult(TicketResolveResult.Resolved)
            {
                TicketId = ticket.TicketId,
                ResolvedBy = ticket.ResolvedBy.Value,
                ResolvedOn = ticket.ResolvedOn.Value,
                UserGuid = ticket.ResolvedBy.Value
            };
        }

        public ResolveTicketResult TicketAlreadyClosed(Ticket ticket)
        {
            return new ResolveTicketResult(TicketResolveResult.TicketAlreadyClosed)
            {
                TicketId = ticket.TicketId,
                ClosedBy = ticket.ClosedBy.Value,
                ClosedOn = ticket.ClosedOn.Value
            };
        }

        public ResolveTicketResult TicketAlreadyResolved(Ticket ticket)
        {
            return new ResolveTicketResult(TicketResolveResult.TicketAlreadyResolved)
            {
                TicketId = ticket.TicketId,
                ResolvedBy = ticket.ResolvedBy.Value,
                ResolvedOn = ticket.ResolvedOn.Value
            };
        }

        public ResolveTicketResult TicketNotFound(int ticketId)
        {
            return new ResolveTicketResult(TicketResolveResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        public ResolveTicketResult WorkflowFailed(int ticketId, Guid userGuid, IWorkflowProcess workflow)
        {
            return new ResolveTicketResult(TicketResolveResult.WorkflowFailed)
            {
                TicketId = ticketId,
                UserGuid = userGuid,
                Workflow = workflow
            };
        }
    }
}