using Helpdesk.Domain.Entities;
using Helpdesk.Services.TicketLinks.Results;

namespace Helpdesk.Services.TicketLinks.Factories.LinkTickets
{
    public interface ILinkTicketsResultFactory
    {
        LinkTicketsResult Linked(TicketLink ticketLink);

        LinkTicketsResult TicketsAlreadyLinked(TicketLink ticketLink);
    }
}