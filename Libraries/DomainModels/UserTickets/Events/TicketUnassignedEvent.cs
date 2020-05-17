namespace Helpdesk.DomainModels.UserTickets.Events
{
    public class TicketUnassignedEvent
    {
        public TicketUnassignedEvent(int ticketId, int userId)
        {
            TicketId = ticketId;
            UserId = userId;
        }

        public int TicketId { get; }
        public int UserId { get; }
    }
}