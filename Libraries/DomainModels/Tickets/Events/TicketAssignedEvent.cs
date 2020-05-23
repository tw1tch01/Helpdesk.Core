using System;

namespace Helpdesk.DomainModels.Tickets.Events
{
    public class TicketAssignedEvent
    {
        public TicketAssignedEvent(int ticketId, Guid userGuid)
        {
            TicketId = ticketId;
            UserGuid = userGuid;
        }

        public int TicketId { get; }
        public Guid UserGuid { get; }
    }
}