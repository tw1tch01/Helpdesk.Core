using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.PauseTicket
{
    public interface IPauseTicketResultFactory
    {
        PauseTicketResult Paused(Ticket ticket);

        PauseTicketResult TicketAlreadyClosed(Ticket ticket);

        PauseTicketResult TicketAlreadyPaused(Ticket ticket);

        PauseTicketResult TicketAlreadyResolved(Ticket ticket);

        PauseTicketResult TicketNotFound(int ticketId);

        PauseTicketResult WorkflowFailed(int ticketId, int userId, IWorkflowProcess workflow);
    }
}