using Helpdesk.DomainModels.UserTickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.UserTickets.Events.AssignTicket
{
    public class BeforeTicketAssignedWorkflow : TicketAssignedEvent, IWorkflowProcess
    {
        public BeforeTicketAssignedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}