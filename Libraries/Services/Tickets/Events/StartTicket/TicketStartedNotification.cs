using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.StartTicket
{
    public class TicketStartedNotification : TicketStartedEvent, INotificationProcess
    {
        public TicketStartedNotification(int ticketId)
            : base(ticketId)
        {
        }
    }
}