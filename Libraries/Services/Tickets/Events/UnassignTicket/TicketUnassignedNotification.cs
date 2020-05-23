using System;
using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.UnassignTicket
{
    public class TicketUnassignedNotification : TicketUnassignedEvent, INotificationProcess
    {
        public TicketUnassignedNotification(int ticketId, Guid userGuid)
            : base(ticketId, userGuid)
        {
        }
    }
}