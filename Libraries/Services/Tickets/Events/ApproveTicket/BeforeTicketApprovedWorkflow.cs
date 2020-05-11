using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.ApproveTicket
{
    public class BeforeTicketApprovedWorkflow : TicketApprovedEvent, IWorkflowProcess
    {
        public BeforeTicketApprovedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}