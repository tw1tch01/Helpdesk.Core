using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.ReopenTicket
{
    public interface IReopenTicketResultFactory
    {
        ReopenTicketResult Reopened(Ticket ticket);

        ReopenTicketResult TicketNotFound(int ticketId);

        ReopenTicketResult WorkflowFailed(int ticketId, int userId, IWorkflowProcess workflow);
    }
}