using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.StartTicket
{
    public interface IStartTicketResultFactory
    {
        StartTicketResult Started(Ticket ticket);

        StartTicketResult TicketAlreadyClosed(Ticket ticket);

        StartTicketResult TicketAlreadyResolved(Ticket ticket);

        StartTicketResult TicketAlreadyStarted(Ticket ticket);

        StartTicketResult TicketNotFound(int ticketId);

        StartTicketResult WorkflowFailed(int ticketId, int userId, IWorkflowProcess workflow);
    }
}