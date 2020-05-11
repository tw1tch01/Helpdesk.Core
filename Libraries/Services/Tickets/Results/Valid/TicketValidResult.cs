namespace Helpdesk.Services.Tickets.Results.Valid
{
    public abstract class TicketValidResult : TicketValidationResult
    {
        protected TicketValidResult(string message)
            : base(true, message)
        {
        }

        protected TicketValidResult(int ticketId, string message)
            : base(ticketId, true, message)
        {
        }
    }
}