namespace Helpdesk.DomainModels.UserTickets.Events
{
    public class TicketAssignedEvent
    {
        public TicketAssignedEvent(int ticketId, int userId)
        {
            TicketId = ticketId;
            UserId = userId;
        }

        public int TicketId { get; }
        public int UserId { get; }
    }
}