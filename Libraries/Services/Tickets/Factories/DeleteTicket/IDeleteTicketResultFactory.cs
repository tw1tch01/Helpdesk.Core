using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.DeleteTicket
{
    public interface IDeleteTicketResultFactory
    {
        DeleteTicketResult TicketNotFound(int ticketId);
        DeleteTicketResult WorkflowFailed(int ticketId, int userId, IWorkflowProcess beforeWorkflow);
    }
}