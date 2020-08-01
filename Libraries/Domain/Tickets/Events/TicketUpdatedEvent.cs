using System.Collections.Generic;
using Helpdesk.Domain.Common;

namespace Helpdesk.Domain.Tickets.Events
{
    public class TicketUpdatedEvent
    {
        public TicketUpdatedEvent(int ticketId)
        {
            TicketId = ticketId;
            Changes = new Dictionary<string, ValueChange>();
        }

        public TicketUpdatedEvent(int ticketId, IReadOnlyDictionary<string, ValueChange> changes)
        {
            TicketId = ticketId;
            Changes = changes;
        }

        public int TicketId { get; }
        public IReadOnlyDictionary<string, ValueChange> Changes { get; }
    }
}