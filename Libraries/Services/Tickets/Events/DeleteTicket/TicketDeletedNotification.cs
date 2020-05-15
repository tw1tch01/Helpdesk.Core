using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.DeleteTicket
{
    public class TicketDeletedNotification : TicketDeletedEvent, INotificationProcess
    {
        public TicketDeletedNotification(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}