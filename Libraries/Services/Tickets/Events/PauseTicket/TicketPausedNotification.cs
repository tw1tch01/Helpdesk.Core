using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.PauseTicket
{
    public class TicketPausedNotification : TicketPausedEvent, INotificationProcess
    {
        public TicketPausedNotification(int ticketId)
            : base(ticketId)
        {
        }
    }
}