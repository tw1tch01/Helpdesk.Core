namespace Helpdesk.DomainModels.Tickets.Events
{
    public class TicketOpenedEvent
    {
        public TicketOpenedEvent(int ticketId)
        {
            TicketId = ticketId;
        }

        public int TicketId { get; }
    }
}