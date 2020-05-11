namespace Helpdesk.Services.Tickets.Results.Valid
{
    public class TicketOpenedResult : TicketValidResult
    {
        private const string _message = "Ticket has been opened.";

        public TicketOpenedResult(int ticketId)
            : base(ticketId, _message)
        {
        }
    }
}