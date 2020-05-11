namespace Helpdesk.Services.UserTickets.Results.Valid
{
    public class UserUnassignedFromTicketResult : UserTicketValidResult
    {
        private const string _message = "User has been unassigned from Ticket.";

        public UserUnassignedFromTicketResult(int ticketId, int userId)
            : base(ticketId, userId, _message)
        {
        }
    }
}