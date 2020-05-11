using Helpdesk.Domain.Enums;

namespace Helpdesk.Services.TicketLinks.Results.Invalid
{
    public abstract class TicketLinkInvalidResult : TicketLinkValidationResult
    {
        protected TicketLinkInvalidResult(string message)
            : base(false, message)
        {
        }

        protected TicketLinkInvalidResult(int fromTicketId, int toTicketId, string message)
            : base(fromTicketId, toTicketId, false, message)
        {
        }

        protected TicketLinkInvalidResult(int fromTicketId, int toTicketId, TicketLinkType linkType, string message)
            : base(fromTicketId, toTicketId, linkType, false, message)
        {
        }
    }
}