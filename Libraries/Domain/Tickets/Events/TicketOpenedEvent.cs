using System;

namespace Helpdesk.Domain.Tickets.Events
{
    public class TicketOpenedEvent
    {
        public TicketOpenedEvent(int ticketId, Guid userGuid)
        {
            TicketId = ticketId;
            UserGuid = userGuid;
        }

        public int TicketId { get; }
        public Guid UserGuid { get; }
    }
}