using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.OpenTicket
{
    public class TicketOpenedNotification : TicketOpenedEvent, INotificationProcess
    {
        public TicketOpenedNotification(int ticketId)
            : base(ticketId)
        {
        }
    }
}