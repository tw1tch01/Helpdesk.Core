using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.CloseTicket
{
    public class TicketClosedWorkflow : TicketClosedEvent, IWorkflowProcess
    {
        public TicketClosedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}