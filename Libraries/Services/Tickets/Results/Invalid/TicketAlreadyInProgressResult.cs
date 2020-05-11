namespace Helpdesk.Services.Tickets.Results.Invalid
{
    public class TicketAlreadyInProgressResult : TicketInvalidResult
    {
        private const string _message = "Ticket is already in progress.";

        public TicketAlreadyInProgressResult(int ticketId)
            : base(ticketId, _message)
        {
        }
    }
}