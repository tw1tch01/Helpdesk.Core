using System;
using Helpdesk.Domain.Common;

namespace Helpdesk.Domain.Tickets.Events
{
    public class TicketStartedEvent : DomainEvent
    {
        public TicketStartedEvent(int ticketId, Guid userGuid)
        {
            TicketId = ticketId;
            UserGuid = userGuid;
        }

        public int TicketId { get; }

        public Guid UserGuid { get; }
    }
}