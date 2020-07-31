using System.Threading.Tasks;
using Helpdesk.DomainModels.TicketLinks;
using Helpdesk.Services.TicketLinks.Results;

namespace Helpdesk.Services.TicketLinks.Commands.LinkTickets
{
    public interface ILinkTicketService
    {
        Task<LinkTicketsResult> Link(NewTicketsLink newLink);
    }
}