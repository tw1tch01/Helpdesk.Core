namespace Helpdesk.Services.Tickets.Results.Invalid
{
    public abstract class TicketInvalidResult : TicketValidationResult
    {
        protected TicketInvalidResult(string message)
            : base(false, message)
        {
        }

        protected TicketInvalidResult(int ticketId, string message)
            : base(ticketId, false, message)
        {
        }
    }
}