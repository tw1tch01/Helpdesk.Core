using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.ReopenTicket
{
    public class BeforeTicketReopenedWorkflow : TicketReopenedEvent, IWorkflowProcess
    {
        public BeforeTicketReopenedWorkflow(int ticketId)
            : base(ticketId)
        {
        }
    }
}