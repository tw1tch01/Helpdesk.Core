using Helpdesk.DomainModels.TicketLinks.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.TicketLinks.Events.UnlinkTicket
{
    public class TicketsUnlinkedNotification : TicketsUnlinkedEvent, INotificationProcess
    {
        public TicketsUnlinkedNotification(int fromTicketId, int toTicketId)
            : base(fromTicketId, toTicketId)
        {
        }
    }
}