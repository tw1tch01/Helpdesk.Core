using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.ResolveTicket
{
    public class BeforeTicketResolvedWorkflow : TicketResolvedEvent, IWorkflowProcess
    {
        public BeforeTicketResolvedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}