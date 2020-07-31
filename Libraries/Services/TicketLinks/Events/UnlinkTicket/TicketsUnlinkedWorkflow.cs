using Helpdesk.DomainModels.TicketLinks.Events;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.TicketLinks.Events.UnlinkTicket
{
    public class TicketsUnlinkedWorkflow : TicketsUnlinkedEvent, IWorkflowProcess
    {
        public TicketsUnlinkedWorkflow(int fromTicketId, int toTicketId)
            : base(fromTicketId, toTicketId)
        {
        }

        public WorkflowResult Result { get; }
        public string Message { get; }
    }
}