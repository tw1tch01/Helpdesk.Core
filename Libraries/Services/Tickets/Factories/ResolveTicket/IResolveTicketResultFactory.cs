using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.ResolveTicket
{
    public interface IResolveTicketResultFactory
    {
        ResolveTicketResult Resolved(Ticket ticket);

        ResolveTicketResult TicketAlreadyResolved(Ticket ticket);

        ResolveTicketResult TicketAlreadyClosed(Ticket ticket);

        ResolveTicketResult TicketNotFound(int ticketId);

        ResolveTicketResult UserNotFound(int ticketId, int userId);

        ResolveTicketResult WorkflowFailed(int ticketId, int userId, IWorkflowProcess workflow);
    }
}