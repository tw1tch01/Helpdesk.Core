using System;
using Helpdesk.Domain.Common;

namespace Helpdesk.Domain.Tickets.Events
{
    public class TicketDeletedEvent : DomainEvent
    {
        public TicketDeletedEvent(int ticketId, Guid userGuid)
        {
            TicketId = ticketId;
            UserGuid = userGuid;
        }

        public int TicketId { get; }
        public Guid UserGuid { get; }
    }
}