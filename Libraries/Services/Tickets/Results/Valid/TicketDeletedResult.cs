namespace Helpdesk.Services.Tickets.Results.Valid
{
    public class TicketDeletedResult : TicketValidResult
    {
        private const string _message = "Ticket has been deleted.";

        public TicketDeletedResult(int ticketId)
            : base(ticketId, _message)
        {
        }
    }
}