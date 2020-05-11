using Helpdesk.DomainModels.UserTickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.UserTickets.Events.UnassignTicket
{
    public class TicketUnassignedWorkflow : TicketUnassignedEvent, IWorkflowProcess
    {
        public TicketUnassignedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}