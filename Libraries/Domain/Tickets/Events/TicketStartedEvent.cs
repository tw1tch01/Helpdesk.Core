using System;

namespace Helpdesk.Domain.Tickets.Events
{
    public class TicketStartedEvent
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