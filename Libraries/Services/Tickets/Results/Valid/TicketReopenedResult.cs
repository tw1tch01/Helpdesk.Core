namespace Helpdesk.Services.Tickets.Results.Valid
{
    public class TicketReopenedResult : TicketValidResult
    {
        private const string _message = "Ticket has been reopened.";

        public TicketReopenedResult(int ticketId)
            : base(ticketId, _message)
        {
        }
    }
}