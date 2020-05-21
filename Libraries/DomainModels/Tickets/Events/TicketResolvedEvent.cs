﻿using System;

namespace Helpdesk.DomainModels.Tickets.Events
{
    public class TicketResolvedEvent
    {
        public TicketResolvedEvent(int ticketId, Guid userGuid)
        {
            TicketId = ticketId;
            UserGuid = userGuid;
        }

        public int TicketId { get; }
        public Guid UserGuid { get; }
    }
}