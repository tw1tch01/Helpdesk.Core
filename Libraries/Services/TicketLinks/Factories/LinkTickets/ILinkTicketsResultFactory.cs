using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Services.TicketLinks.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.TicketLinks.Factories.LinkTickets
{
    public interface ILinkTicketsResultFactory
    {
        LinkTicketsResult Linked(TicketLink ticketLink);

        LinkTicketsResult TicketsAlreadyLinked(TicketLink ticketLink);

        LinkTicketsResult WorkflowFailed(int fromTicketId, int toTicketId, TicketLinkType linkType, IWorkflowProcess workflow);
    }
}