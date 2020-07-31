using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Services.TicketLinks.Results;
using Helpdesk.Services.TicketLinks.Results.Enums;
using Helpdesk.Services.Workflows;

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

        public LinkTicketsResult WorkflowFailed(int fromTicketId, int toTicketId, TicketLinkType linkType, IWorkflowProcess workflow)
        {
            return new LinkTicketsResult(TicketsLinkResult.TicketsAlreadyLinked)
            {
                FromTicketId = fromTicketId,
                ToTicketId = toTicketId,
                LinkType = linkType,
                Workflow = workflow
            };
        }
    }
}