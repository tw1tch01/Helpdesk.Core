using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Events.StartTicket
{
    public class TicketStartedWorkflow : TicketStartedEvent, IWorkflowProcess
    {
        public TicketStartedWorkflow(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }

        public WorkflowResult Result { get; set; }
        public string Message { get; set; }
    }
}