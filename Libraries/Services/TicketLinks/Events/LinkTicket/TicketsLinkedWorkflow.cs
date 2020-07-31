using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.DomainModels.TicketLinks.Events;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.TicketLinks.Events.LinkTicket
{
    public class TicketsLinkedWorkflow : TicketsLinkedEvent, IWorkflowProcess
    {
        public TicketsLinkedWorkflow(int fromTicketId, int toTicketId, TicketLinkType linkType)
            : base(fromTicketId, toTicketId, linkType)
        {
        }

        public WorkflowResult Result { get; }
        public string Message { get; }
    }
}