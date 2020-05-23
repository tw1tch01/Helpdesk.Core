using System;

namespace Helpdesk.DomainModels.Tickets.Events
{
    public class TicketPausedEvent
    {
        public TicketPausedEvent(int ticketId, Guid userGuid)
        {
            TicketId = ticketId;
            UserGuid = userGuid;
        }

        public int TicketId { get; }
        public Guid UserGuid { get; }
    }
}