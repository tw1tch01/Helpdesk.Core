using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.DeleteTicket
{
    public class TicketDeletedWorkflow : TicketDeletedEvent, IWorkflowProcess
    {
        public TicketDeletedWorkflow(int ticketId)
            : base(ticketId)
        {
        }
    }
}