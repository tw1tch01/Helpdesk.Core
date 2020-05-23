using System;
using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.AssignTicket
{
    public class TicketAssignedNotification : TicketAssignedEvent, INotificationProcess
    {
        public TicketAssignedNotification(int ticketId, Guid userGuid)
            : base(ticketId, userGuid)
        {
        }
    }
}
