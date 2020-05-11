using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.StartTicket
{
    public class TicketStartedWorkflow : TicketStartedEvent, IWorkflowProcess
    {
        public TicketStartedWorkflow(int ticketId)
            : base(ticketId)
        {
        }
    }
}