using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.StartTicket
{
    public class StartTicketResultFactory : IStartTicketResultFactory
    {
        public StartTicketResult Started(Ticket ticket)
        {
            return new StartTicketResult(TicketStartResult.Started)
            {
                TicketId = ticket.TicketId,
                StartedOn = ticket.StartedOn.Value
            };
        }

        public StartTicketResult TicketAlreadyClosed(Ticket ticket)
        {
            return new StartTicketResult(TicketStartResult.TicketAlreadyClosed)
            {
                TicketId = ticket.TicketId,
                ClosedBy = ticket.ClosedBy.Value,
                ClosedOn = ticket.ClosedOn.Value,
            };
        }

        public StartTicketResult TicketAlreadyResolved(Ticket ticket)
        {
            return new StartTicketResult(TicketStartResult.TicketAlreadyResolved)
            {
                TicketId = ticket.TicketId,
                ResolvedBy = ticket.ResolvedBy.Value,
                ResolvedOn = ticket.ResolvedOn.Value,
            };
        }

        public StartTicketResult TicketAlreadyStarted(Ticket ticket)
        {
            return new StartTicketResult(TicketStartResult.TicketAlreadyStarted)
            {
                TicketId = ticket.TicketId,
                StartedOn = ticket.StartedOn.Value
            };
        }

        public StartTicketResult TicketNotFound(int ticketId)
        {
            return new StartTicketResult(TicketStartResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        public StartTicketResult WorkflowFailed(int ticketId, int userId, IWorkflowProcess workflow)
        {
            return new StartTicketResult(TicketStartResult.WorkflowFailed)
            {
                TicketId = ticketId,
                UserId = userId,
                Workflow = workflow
            };
        }
    }
}