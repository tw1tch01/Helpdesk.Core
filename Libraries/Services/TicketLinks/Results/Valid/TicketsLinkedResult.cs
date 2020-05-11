using Helpdesk.Domain.Enums;

namespace Helpdesk.Services.TicketLinks.Results.Valid
{
    public class TicketsLinkedResult : TicketLinkValidResult
    {
        private const string _message = "Tickets have been linked.";

        public TicketsLinkedResult(int fromTicketId, int toTicketId, TicketLinkType linkType)
            : base(fromTicketId, toTicketId, linkType, _message)
        {
        }
    }
}