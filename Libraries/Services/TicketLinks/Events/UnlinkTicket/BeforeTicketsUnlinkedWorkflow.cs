using Helpdesk.DomainModels.TicketLinks.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.TicketLinks.Events.UnlinkTicket
{
    public class BeforeTicketsUnlinkedWorkflow : TicketsUnlinkedEvent, IWorkflowProcess
    {
        public BeforeTicketsUnlinkedWorkflow(int fromTicketId, int toTicketId)
            : base(fromTicketId, toTicketId)
        {
        }
    }
}