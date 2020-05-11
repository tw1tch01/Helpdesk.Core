using System.Collections.Generic;
using Helpdesk.DomainModels.Common;
using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Events.UpdateTicket
{
    public class BeforeTicketUpdateWorkflow : TicketUpdateEvent, IWorkflowProcess
    {
        public BeforeTicketUpdateWorkflow(int ticketId, IReadOnlyDictionary<string, ValueChange> changes)
            : base(ticketId, changes)
        {
        }
    }
}