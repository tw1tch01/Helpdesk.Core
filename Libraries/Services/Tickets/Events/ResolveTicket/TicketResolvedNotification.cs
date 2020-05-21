using System;
using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.ResolveTicket
{
    public class TicketResolvedNotification : TicketResolvedEvent, INotificationProcess
    {
        public TicketResolvedNotification(int ticketId, Guid userGuid)
            : base(ticketId, userGuid)
        {
        }
    }
}