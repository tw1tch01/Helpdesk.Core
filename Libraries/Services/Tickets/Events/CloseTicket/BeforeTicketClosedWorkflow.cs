using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Events.CloseTicket
{
    public class BeforeTicketClosedWorkflow : TicketClosedEvent, IWorkflowProcess
    {
        public BeforeTicketClosedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }

        public WorkflowResult Result { get; set; }
        public string Message { get; set; }
    }
}