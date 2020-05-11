using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.OpenTicket
{
    public class TicketOpenedWorkflow : TicketOpenedEvent, IWorkflowProcess
    {
        public TicketOpenedWorkflow(int ticketId)
            : base(ticketId)
        {
        }
    }
}