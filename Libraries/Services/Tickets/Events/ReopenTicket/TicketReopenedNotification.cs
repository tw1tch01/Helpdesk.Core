using System;
using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.ReopenTicket
{
    public class TicketReopenedNotification : TicketReopenedEvent, INotificationProcess
    {
        public TicketReopenedNotification(int ticketId, Guid userGuid)
            : base(ticketId, userGuid)
        {
        }
    }
}