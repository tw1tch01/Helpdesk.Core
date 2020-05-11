using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.ResolveTicket
{
    public class TicketResolvedWorkflow : TicketResolvedEvent, IWorkflowProcess
    {
        public TicketResolvedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}