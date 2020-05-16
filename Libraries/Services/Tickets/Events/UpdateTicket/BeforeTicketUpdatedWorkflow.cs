using System.Collections.Generic;
using Helpdesk.DomainModels.Common;
using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Events.UpdateTicket
{
    public class BeforeTicketUpdatedWorkflow : TicketUpdateEvent, IWorkflowProcess
    {
        public BeforeTicketUpdatedWorkflow(int ticketId, IReadOnlyDictionary<string, ValueChange> changes)
            : base(ticketId, changes)
        {
        }

        public virtual WorkflowResult Result { get; set; }
        public virtual string Message { get; set; }
    }
}