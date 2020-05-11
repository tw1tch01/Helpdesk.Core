using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.ReopenTicket
{
    public class TicketReopenedWorkflow : TicketPausedEvent, IWorkflowProcess
    {
        public TicketReopenedWorkflow(int ticketId)
            : base(ticketId)
        {
        }
    }
}