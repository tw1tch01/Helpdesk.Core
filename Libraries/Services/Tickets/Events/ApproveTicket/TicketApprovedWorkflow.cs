using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.ApproveTicket
{
    public class TicketApprovedWorkflow : TicketApprovedEvent, IWorkflowProcess
    {
        public TicketApprovedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}