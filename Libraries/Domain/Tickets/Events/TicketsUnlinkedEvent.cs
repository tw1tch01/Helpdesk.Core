using Helpdesk.Domain.Common;

namespace Helpdesk.Domain.Tickets.Events
{
    public class TicketsUnlinkedEvent : DomainEvent
    {
        public TicketsUnlinkedEvent(int fromTicketId, int toTicketId)
        {
            FromTicketId = fromTicketId;
            ToTicketId = toTicketId;
        }

        public int FromTicketId { get; }
        public int ToTicketId { get; }
    }
}