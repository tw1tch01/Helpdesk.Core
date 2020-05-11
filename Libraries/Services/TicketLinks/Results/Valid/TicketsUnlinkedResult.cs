namespace Helpdesk.Services.TicketLinks.Results.Valid
{
    public class TicketsUnlinkedResult : TicketLinkValidResult
    {
        private const string _message = "Tickets have been unlinked.";

        public TicketsUnlinkedResult(int fromTicketId, int toTicketId)
            : base(fromTicketId, toTicketId, _message)
        {
        }
    }
}