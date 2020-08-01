using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.TicketLinks.Results.Enums;

namespace Helpdesk.Services.TicketLinks.Results
{
    public class LinkTicketsResult : IProcessResult<TicketsLinkResult>
    {
        public LinkTicketsResult(TicketsLinkResult result)
        {
            Result = result;
        }

        public TicketsLinkResult Result { get; }
        public string Message => GetMessage();
        public int FromTicketId { get; set; }
        public int ToTicketId { get; set; }
        public TicketLinkType LinkType { get; set; }

        private string GetMessage() => Result switch
        {
            TicketsLinkResult.Linked => ResultMessages.Linked,
            TicketsLinkResult.TicketsAlreadyLinked => ResultMessages.TicketsAlreadyLinked,
            _ => Result.ToString(),
        };
    }
}