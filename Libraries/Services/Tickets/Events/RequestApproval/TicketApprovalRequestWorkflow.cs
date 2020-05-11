using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.RequestApproval
{
    public class TicketApprovalRequestWorkflow : TicketApprovalRequestEvent, IWorkflowProcess
    {
        public TicketApprovalRequestWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}