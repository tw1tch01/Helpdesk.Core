using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Factories.AssignTicket
{
    public interface IAssignTicketResultFactory
    {
        AssignTicketResult Assigned(Ticket ticket);

        AssignTicketResult TicketAlreadyClosed(Ticket ticket);

        AssignTicketResult TicketAlreadyResolved(Ticket ticket);

        AssignTicketResult TicketNotFound(int ticketId);
    }
}