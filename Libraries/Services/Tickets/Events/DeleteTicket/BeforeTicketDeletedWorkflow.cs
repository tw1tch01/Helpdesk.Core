using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Events.DeleteTicket
{
    public class BeforeTicketDeletedWorkflow : TicketDeletedEvent, IWorkflowProcess
    {
        public BeforeTicketDeletedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }

        public virtual WorkflowResult Result { get; set; }
        public virtual string Message { get; set; }
    }
}