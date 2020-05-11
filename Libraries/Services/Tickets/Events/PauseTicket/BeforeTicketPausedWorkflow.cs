using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.PauseTicket
{
    public class BeforeTicketPausedWorkflow : TicketPausedEvent, IWorkflowProcess
    {
        public BeforeTicketPausedWorkflow(int ticketId)
            : base(ticketId)
        {
        }
    }
}