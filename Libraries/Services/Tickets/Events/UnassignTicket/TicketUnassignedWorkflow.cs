using System;
using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Events.UnassignTicket
{
    public class TicketUnassignedWorkflow : TicketUnassignedEvent, IWorkflowProcess
    {
        public TicketUnassignedWorkflow(int ticketId, Guid userGuid)
            : base(ticketId, userGuid)
        {
        }

        public WorkflowResult Result { get; set; }
        public string Message { get; set; }
    }
}