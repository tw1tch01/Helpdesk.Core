namespace Helpdesk.Services.Tickets.Results.Valid
{
    public class TicketPausedResult : TicketValidResult
    {
        private const string _message = "Ticket has been paused.";

        public TicketPausedResult(int ticketId)
            : base(ticketId, _message)
        {
        }
    }
}