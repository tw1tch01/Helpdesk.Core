namespace Helpdesk.Services.TicketLinks.Results.Invalid
{
    public class TicketsNotLinkedResult : TicketLinkInvalidResult
    {
        private const string _message = "Tickets are not linked.";

        public TicketsNotLinkedResult(int fromTicketId, int toTicketId)
            : base(fromTicketId, toTicketId, _message)
        {
        }
    }
}