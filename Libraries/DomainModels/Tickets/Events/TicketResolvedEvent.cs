namespace Helpdesk.DomainModels.Tickets.Events
{
    public class TicketResolvedEvent
    {
        public TicketResolvedEvent(int ticketId, int userId)
        {
            TicketId = ticketId;
            UserId = userId;
        }

        public int TicketId { get; }
        public int UserId { get; }
    }
}