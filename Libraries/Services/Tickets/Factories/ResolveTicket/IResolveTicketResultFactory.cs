using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Factories.ResolveTicket
{
    public interface IResolveTicketResultFactory
    {
        ResolveTicketResult Resolved(Ticket ticket);

        ResolveTicketResult TicketAlreadyResolved(Ticket ticket);

        ResolveTicketResult TicketAlreadyClosed(Ticket ticket);

        ResolveTicketResult TicketNotFound(int ticketId);
    }
}