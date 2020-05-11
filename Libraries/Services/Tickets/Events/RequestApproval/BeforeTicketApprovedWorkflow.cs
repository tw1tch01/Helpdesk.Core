using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.RequestApproval
{
    public class BeforeTicketApprovalRequestProcess : TicketApprovalRequestEvent, IWorkflowProcess
    {
        public BeforeTicketApprovalRequestProcess(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}