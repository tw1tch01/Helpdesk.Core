using Helpdesk.DomainModels.TicketLinks.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.TicketLinks.Events.UnlinkTicket
{
    public class TicketsUnlinkedWorkflow : TicketsUnlinkedEvent, IWorkflowProcess
    {
        public TicketsUnlinkedWorkflow(int fromTicketId, int toTicketId)
            : base(fromTicketId, toTicketId)
        {
        }
    }
}