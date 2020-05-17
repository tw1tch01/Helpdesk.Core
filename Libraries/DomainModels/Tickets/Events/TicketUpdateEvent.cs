using System.Collections.Generic;
using Helpdesk.DomainModels.Common;

namespace Helpdesk.DomainModels.Tickets.Events
{
    public class TicketUpdateEvent
    {
        public TicketUpdateEvent(int ticketId)
        {
            TicketId = ticketId;
            Changes = new Dictionary<string, ValueChange>();
        }

        public TicketUpdateEvent(int ticketId, IReadOnlyDictionary<string, ValueChange> changes)
        {
            TicketId = ticketId;
            Changes = changes;
        }

        public int TicketId { get; }
        public IReadOnlyDictionary<string, ValueChange> Changes { get; }
    }
}