using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Factories.CloseTicket
{
    public class CloseTicketResultFactory : ICloseTicketResultFactory
    {
        public CloseTicketResult Closed(Ticket ticket)
        {
            return new CloseTicketResult(TicketCloseResult.Closed)
            {
                TicketId = ticket.TicketId,
                UserGuid = ticket.ClosedBy.Value,
                ClosedBy = ticket.ClosedBy.Value,
                ClosedOn = ticket.ClosedOn.Value
            };
        }

        public CloseTicketResult TicketAlreadyClosed(Ticket ticket)
        {
            return new CloseTicketResult(TicketCloseResult.TicketAlreadyClosed)
            {
                TicketId = ticket.TicketId,
                ClosedBy = ticket.ClosedBy.Value,
                ClosedOn = ticket.ClosedOn.Value
            };
        }

        public CloseTicketResult TicketAlreadyResolved(Ticket ticket)
        {
            return new CloseTicketResult(TicketCloseResult.TicketAlreadyResolved)
            {
                TicketId = ticket.TicketId,
                ResolvedBy = ticket.ResolvedBy.Value,
                ResolvedOn = ticket.ResolvedOn.Value
            };
        }

        public CloseTicketResult TicketNotFound(int ticketId)
        {
            return new CloseTicketResult(TicketCloseResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }
    }
}