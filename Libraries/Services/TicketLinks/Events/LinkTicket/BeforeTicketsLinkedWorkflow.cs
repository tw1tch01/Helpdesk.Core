using Helpdesk.Domain.Enums;
using Helpdesk.DomainModels.TicketLinks.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.TicketLinks.Events.LinkTicket
{
    public class BeforeTicketsLinkedWorkflow : TicketsLinkedEvent, IWorkflowProcess
    {
        public BeforeTicketsLinkedWorkflow(int fromTicketId, int toTicketId, TicketLinkType linkType)
            : base(fromTicketId, toTicketId, linkType)
        {
        }
    }
}