namespace Helpdesk.Services.UserTickets.Results.Invalid
{
    public class UserAlreadyAssignedToTicketResult : UserTicketInvalidResult
    {
        private const string _message = "User has already been assigned to this ticket.";

        public UserAlreadyAssignedToTicketResult(int ticketId, int userId)
            : base(ticketId, userId, _message)
        {
        }
    }
}