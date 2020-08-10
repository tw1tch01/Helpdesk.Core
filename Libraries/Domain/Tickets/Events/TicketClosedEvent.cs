using System;
using Helpdesk.Domain.Common;

namespace Helpdesk.Domain.Tickets.Events
{
    public class TicketClosedEvent : DomainEvent
    {
        public TicketClosedEvent(int ticketId, Guid userGuid)
        {
            TicketId = ticketId;
            UserGuid = userGuid;
        }

        public int TicketId { get; }
        public Guid UserGuid { get; }
    }
}