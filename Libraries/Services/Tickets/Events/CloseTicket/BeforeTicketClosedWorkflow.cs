using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.CloseTicket
{
    public class BeforeTicketClosedWorkflow : TicketClosedEvent, IWorkflowProcess
    {
        public BeforeTicketClosedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}