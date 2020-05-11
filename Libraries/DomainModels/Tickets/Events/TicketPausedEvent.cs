namespace Helpdesk.DomainModels.Tickets.Events
{
    public class TicketPausedEvent
    {
        public TicketPausedEvent(int ticketId)
        {
            TicketId = ticketId;
        }

        public int TicketId { get; }
    }
}