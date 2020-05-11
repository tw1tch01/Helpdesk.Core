namespace Helpdesk.Services.Tickets.Results.Valid
{
    public class TicketStartedResult : TicketValidResult
    {
        private const string _message = "Ticket has been started.";

        public TicketStartedResult(int ticketId)
            : base(ticketId, _message)
        {
        }
    }
}