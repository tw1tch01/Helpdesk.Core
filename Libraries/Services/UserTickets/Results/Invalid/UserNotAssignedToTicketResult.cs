namespace Helpdesk.Services.UserTickets.Results.Invalid
{
    public class UserNotAssignedToTicketResult : UserTicketInvalidResult
    {
        private const string _message = "User is not assigned to this ticket.";

        public UserNotAssignedToTicketResult(int ticketId, int userId)
            : base(ticketId, userId, _message)
        {
        }
    }
}