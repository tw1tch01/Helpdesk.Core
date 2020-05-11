namespace Helpdesk.Services.Tickets.Results.Invalid
{
    public class TicketAlreadyOnHoldResult : TicketInvalidResult
    {
        private const string _message = "Ticket already on hold.";

        public TicketAlreadyOnHoldResult(int ticketId)
            : base(ticketId, _message)
        {
        }
    }
}