using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Events.ResolveTicket
{
    public class BeforeTicketResolvedWorkflow : TicketResolvedEvent, IWorkflowProcess
    {
        public BeforeTicketResolvedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }

        public virtual WorkflowResult Result { get; set; }
        public virtual string Message { get; set; }
    }
}