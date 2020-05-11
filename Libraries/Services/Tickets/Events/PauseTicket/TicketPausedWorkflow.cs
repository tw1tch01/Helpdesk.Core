using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.PauseTicket
{
    public class TicketPausedWorkflow : TicketPausedEvent, IWorkflowProcess
    {
        public TicketPausedWorkflow(int ticketId)
            : base(ticketId)
        {
        }
    }
}