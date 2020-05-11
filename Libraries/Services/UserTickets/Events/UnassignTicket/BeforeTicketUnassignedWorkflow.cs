using Helpdesk.DomainModels.UserTickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.UserTickets.Events.UnassignTicket
{
    public class BeforeTicketUnassignedWorkflow : TicketUnassignedEvent, IWorkflowProcess
    {
        public BeforeTicketUnassignedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}