using System.Collections.Generic;
using Helpdesk.DomainModels.Common;
using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Events.UpdateTicket
{
    public class TicketUpdatedWorkflow : TicketUpdateEvent, IWorkflowProcess
    {
        public TicketUpdatedWorkflow(int ticketId, IReadOnlyDictionary<string, ValueChange> changes)
            : base(ticketId, changes)
        {
        }

        public WorkflowResult Result { get; set; }
        public string Message { get; set; }
    }
}