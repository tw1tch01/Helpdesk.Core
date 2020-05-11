namespace Helpdesk.Services.UserTickets.Results.Valid
{
    public class UserAssignedToTicketResult : UserTicketValidResult
    {
        private const string _message = "User has been assigned to Ticket.";

        public UserAssignedToTicketResult(int ticketId, int userId)
            : base(ticketId, userId, _message)
        {
        }
    }
}