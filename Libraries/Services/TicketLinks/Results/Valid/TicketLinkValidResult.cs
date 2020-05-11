using Helpdesk.Domain.Enums;

namespace Helpdesk.Services.TicketLinks.Results.Valid
{
    public abstract class TicketLinkValidResult : TicketLinkValidationResult
    {
        protected TicketLinkValidResult(string message)
            : base(true, message)
        {
        }

        protected TicketLinkValidResult(int fromTicketId, int toTicketId, string message)
            : base(fromTicketId, toTicketId, true, message)
        {
        }

        protected TicketLinkValidResult(int fromTicketId, int toTicketId, TicketLinkType linkType, string message)
            : base(fromTicketId, toTicketId, linkType, true, message)
        {
        }
    }
}