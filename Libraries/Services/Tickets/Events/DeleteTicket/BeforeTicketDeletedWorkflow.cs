using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.DeleteTicket
{
    public class BeforeTicketDeletedWorkflow : TicketDeletedEvent, IWorkflowProcess
    {
        public BeforeTicketDeletedWorkflow(int ticketId)
            : base(ticketId)
        {
        }
    }
}