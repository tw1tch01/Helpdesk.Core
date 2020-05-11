using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.TicketLinks.Results
{
    public abstract class TicketLinkValidationResult : ProcessResult
    {
        private const string _fromTicketIdKey = nameof(TicketLink.FromTicketId);
        private const string _toTicketIdKey = nameof(TicketLink.ToTicketId);
        private const string _linkTypeKey = nameof(TicketLink.LinkType);

        protected TicketLinkValidationResult(bool isValid, string message)
            : base(isValid, message)
        {
        }

        protected TicketLinkValidationResult(int fromTicketId, int toTicketId, bool isValid, string message)
            : base(isValid, message)
        {
            Data[_fromTicketIdKey] = fromTicketId;
            Data[_toTicketIdKey] = toTicketId;
        }

        protected TicketLinkValidationResult(int fromTicketId, int toTicketId, TicketLinkType linkType, bool isValid, string message)
            : base(isValid, message)
        {
            Data[_fromTicketIdKey] = fromTicketId;
            Data[_toTicketIdKey] = toTicketId;
            Data[_linkTypeKey] = linkType;
        }
    }
}