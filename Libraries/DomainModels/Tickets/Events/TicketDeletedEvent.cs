namespace Helpdesk.DomainModels.Tickets.Events
{
    public class TicketDeletedEvent
    {
        public TicketDeletedEvent(int ticketId)
        {
            TicketId = ticketId;
        }

        public int TicketId { get; }
    }
}