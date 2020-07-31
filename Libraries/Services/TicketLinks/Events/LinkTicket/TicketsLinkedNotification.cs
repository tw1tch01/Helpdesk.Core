using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.DomainModels.TicketLinks.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.TicketLinks.Events.LinkTicket
{
    public class TicketsLinkedNotification : TicketsLinkedEvent, INotificationProcess
    {
        public TicketsLinkedNotification(int fromTicketId, int toTicketId, TicketLinkType linkType)
            : base(fromTicketId, toTicketId, linkType)
        {
        }
    }
}