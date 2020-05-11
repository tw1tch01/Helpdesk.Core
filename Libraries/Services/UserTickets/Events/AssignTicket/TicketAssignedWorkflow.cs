using Helpdesk.DomainModels.UserTickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.UserTickets.Events.AssignTicket
{
    public class TicketAssignedWorkflow : TicketAssignedEvent, IWorkflowProcess
    {
        public TicketAssignedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}