using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.ResolveTicket
{
    public class TicketResolvedNotification : TicketResolvedEvent, INotificationProcess
    {
        public TicketResolvedNotification(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}