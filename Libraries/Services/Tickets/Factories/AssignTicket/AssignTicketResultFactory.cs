using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Factories.AssignTicket
{
    public class AssignTicketResultFactory : IAssignTicketResultFactory
    {
        public AssignTicketResult Assigned(Ticket ticket)
        {
            return new AssignTicketResult(TicketAssignResult.Assigned)
            {
                TicketId = ticket.TicketId,
                UserGuid = ticket.AssignedUserGuid.Value
            };
        }

        public AssignTicketResult TicketAlreadyClosed(Ticket ticket)
        {
            return new AssignTicketResult(TicketAssignResult.TicketAlreadyClosed)
            {
                TicketId = ticket.TicketId,
                UserGuid = ticket.AssignedUserGuid.Value,
                ClosedBy = ticket.ClosedBy.Value,
                ClosedOn = ticket.ClosedOn.Value
            };
        }

        public AssignTicketResult TicketAlreadyResolved(Ticket ticket)
        {
            return new AssignTicketResult(TicketAssignResult.TicketAlreadyResolved)
            {
                TicketId = ticket.TicketId,
                UserGuid = ticket.AssignedUserGuid.Value,
                ResolvedBy = ticket.ResolvedBy.Value,
                ResolvedOn = ticket.ResolvedOn.Value
            };
        }

        public AssignTicketResult TicketNotFound(int ticketId)
        {
            return new AssignTicketResult(TicketAssignResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }
    }
}