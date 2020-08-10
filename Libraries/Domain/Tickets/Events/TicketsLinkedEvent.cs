using Helpdesk.Domain.Common;
using Helpdesk.Domain.Tickets.Enums;

namespace Helpdesk.Domain.Tickets.Events
{
    public class TicketsLinkedEvent : DomainEvent
    {
        public TicketsLinkedEvent(int fromTicketId, int toTicketId, TicketLinkType linkType)
        {
            FromTicketId = fromTicketId;
            ToTicketId = toTicketId;
            LinkType = linkType;
        }

        public int FromTicketId { get; }
        public int ToTicketId { get; }
        public TicketLinkType LinkType { get; }
    }
}