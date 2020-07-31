using Helpdesk.Services.TicketLinks.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.TicketLinks.Factories.UnlinkTickets
{
    public interface IUnlinkTicketsResultFactory
    {
        UnlinkTicketsResult TicketsNotLinked(int fromTicketId, int toTicketId);

        UnlinkTicketsResult Unlinked(int fromTicketId, int toTicketId);

        UnlinkTicketsResult WorkflowFailed(int fromTicketId, int toTicketId, IWorkflowProcess workflow);
    }
}