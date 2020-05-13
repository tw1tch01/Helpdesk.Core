using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Events.ReopenTicket
{
    public class TicketReopenedWorkflow : TicketPausedEvent, IWorkflowProcess
    {
        public TicketReopenedWorkflow(int ticketId)
            : base(ticketId)
        {
        }

        public WorkflowResult Result { get; set; }
        public string Message { get; set; }
    }
}