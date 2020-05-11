namespace Helpdesk.Services.UserTickets.Results.Invalid
{
    public abstract class UserTicketInvalidResult : UserTicketValidationResult
    {
        protected UserTicketInvalidResult(string message)
            : base(false, message)
        {
        }

        protected UserTicketInvalidResult(int ticketId, int userId, string message)
            : base(ticketId, userId, false, message)
        {
        }
    }
}