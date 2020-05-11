namespace Helpdesk.Services.Tickets.Results.Invalid
{
    public class TicketNotFoundResult : TicketInvalidResult
    {
        private const string _message = "Ticket record was not found.";

        public TicketNotFoundResult(int ticketId)
            : base(ticketId, _message)
        {
        }
    }
}