namespace Helpdesk.DomainModels.Tickets.Events
{
    public class TicketStartedEvent
    {
        public TicketStartedEvent(int ticketId)
        {
            TicketId = ticketId;
        }

        public int TicketId { get; }
    }
}