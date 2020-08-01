using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Factories.PauseTicket
{
    public interface IPauseTicketResultFactory
    {
        PauseTicketResult Paused(Ticket ticket);

        PauseTicketResult TicketAlreadyClosed(Ticket ticket);

        PauseTicketResult TicketAlreadyPaused(Ticket ticket);

        PauseTicketResult TicketAlreadyResolved(Ticket ticket);

        PauseTicketResult TicketNotFound(int ticketId);
    }
}