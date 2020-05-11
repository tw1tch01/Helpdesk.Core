using Helpdesk.DomainModels.UserTickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.UserTickets.Events.UnassignTicket
{
    public class TicketUnassignedNotification : TicketUnassignedEvent, INotificationProcess
    {
        public TicketUnassignedNotification(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}