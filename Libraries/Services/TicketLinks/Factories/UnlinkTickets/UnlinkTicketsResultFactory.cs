using Helpdesk.Services.TicketLinks.Results;
using Helpdesk.Services.TicketLinks.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.TicketLinks.Factories.UnlinkTickets
{
    public class UnlinkTicketsResultFactory : IUnlinkTicketsResultFactory
    {
        public UnlinkTicketsResult TicketsNotLinked(int fromTicketId, int toTicketId)
        {
            return new UnlinkTicketsResult(TicketsUnlinkResult.TicketsNotLinked)
            {
                FromTicketId = fromTicketId,
                ToTicketId = toTicketId
            };
        }

        public UnlinkTicketsResult Unlinked(int fromTicketId, int toTicketId)
        {
            return new UnlinkTicketsResult(TicketsUnlinkResult.Unlinked)
            {
                FromTicketId = fromTicketId,
                ToTicketId = toTicketId
            };
        }

        public UnlinkTicketsResult WorkflowFailed(int fromTicketId, int toTicketId, IWorkflowProcess workflow)
        {
            return new UnlinkTicketsResult(TicketsUnlinkResult.Unlinked)
            {
                FromTicketId = fromTicketId,
                ToTicketId = toTicketId,
                Workflow = workflow
            };
        }
    }
}