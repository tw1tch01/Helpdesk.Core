using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.StartTicket
{
    public class BeforeTicketStartedWorkflow : TicketStartedEvent, IWorkflowProcess
    {
        public BeforeTicketStartedWorkflow(int ticketId)
            : base(ticketId)
        {
        }
    }
}