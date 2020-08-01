using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Factories.CloseTicket
{
    public interface ICloseTicketResultFactory
    {
        CloseTicketResult Closed(Ticket ticket);

        CloseTicketResult TicketAlreadyClosed(Ticket ticket);

        CloseTicketResult TicketAlreadyResolved(Ticket ticket);

        CloseTicketResult TicketNotFound(int ticketId);
    }
}