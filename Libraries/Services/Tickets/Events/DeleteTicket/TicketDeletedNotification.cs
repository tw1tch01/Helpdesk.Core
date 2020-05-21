using System;
using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.DeleteTicket
{
    public class TicketDeletedNotification : TicketDeletedEvent, INotificationProcess
    {
        public TicketDeletedNotification(int ticketId, Guid userGuid)
            : base(ticketId, userGuid)
        {
        }
    }
}