namespace Helpdesk.DomainModels.Tickets.Events
{
    public class TicketReopenedEvent
    {
        public TicketReopenedEvent(int ticketId)
        {
            TicketId = ticketId;
        }

        public int TicketId { get; }
    }
}