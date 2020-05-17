namespace Helpdesk.DomainModels.TicketLinks.Events
{
    public class TicketsUnlinkedEvent
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