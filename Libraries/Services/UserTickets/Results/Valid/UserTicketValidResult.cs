namespace Helpdesk.Services.UserTickets.Results.Valid
{
    public abstract class UserTicketValidResult : UserTicketValidationResult
    {
        protected UserTicketValidResult(int ticketId, int userId, string message)
            : base(ticketId, userId, true, message)
        {
        }
    }
}