using System;

namespace Helpdesk.DomainModels.Tickets.Events
{
    public class TicketClosedEvent
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