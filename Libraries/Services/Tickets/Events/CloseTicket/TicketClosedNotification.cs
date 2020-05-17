using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.CloseTicket
{
    public class TicketClosedNotification : TicketClosedEvent, INotificationProcess
    {
        public TicketClosedNotification(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}