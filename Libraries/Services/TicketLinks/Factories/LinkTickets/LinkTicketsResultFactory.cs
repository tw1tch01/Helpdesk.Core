using Helpdesk.Domain.Tickets;
using Helpdesk.Services.TicketLinks.Results;
using Helpdesk.Services.TicketLinks.Results.Enums;

namespace Helpdesk.Services.TicketLinks.Factories.LinkTickets
{
    public class LinkTicketsResultFactory : ILinkTicketsResultFactory
    {
        public LinkTicketsResult Linked(TicketLink ticketLink)
        {
            return new LinkTicketsResult(TicketsLinkResult.Linked)
            {
                FromTicketId = ticketLink.FromTicketId,
                ToTicketId = ticketLink.ToTicketId,
                LinkType = ticketLink.LinkType
            };
        }

        public LinkTicketsResult TicketsAlreadyLinked(TicketLink ticketLink)
        {
            return new LinkTicketsResult(TicketsLinkResult.TicketsAlreadyLinked)
            {
                FromTicketId = ticketLink.FromTicketId,
                ToTicketId = ticketLink.ToTicketId,
                LinkType = ticketLink.LinkType
            };
        }
    }
}