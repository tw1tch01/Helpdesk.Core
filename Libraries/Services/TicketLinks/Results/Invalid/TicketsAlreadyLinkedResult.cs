using Helpdesk.Domain.Enums;

namespace Helpdesk.Services.TicketLinks.Results.Invalid
{
    public class TicketsAlreadyLinkedResult : TicketLinkInvalidResult
    {
        private const string _message = "Tickets are already linked.";

        public TicketsAlreadyLinkedResult(int fromTicketId, int toTicketId, TicketLinkType linkType)
            : base(fromTicketId, toTicketId, linkType, _message)
        {
        }
    }
}