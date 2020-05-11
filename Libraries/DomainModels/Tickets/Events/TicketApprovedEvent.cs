namespace Helpdesk.DomainModels.Tickets.Events
{
    public class TicketApprovedEvent
    {
        public TicketApprovedEvent(int ticketId, int userId)
        {
            TicketId = ticketId;
            UserId = userId;
        }

        public int TicketId { get; }
        public int UserId { get; }
    }
}