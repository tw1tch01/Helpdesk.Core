using Helpdesk.Domain.Enums;

namespace Helpdesk.DomainModels.TicketLinks.Events
{
    public class TicketsLinkedEvent
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