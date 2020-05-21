using System;

namespace Helpdesk.DomainModels.Tickets.Events
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