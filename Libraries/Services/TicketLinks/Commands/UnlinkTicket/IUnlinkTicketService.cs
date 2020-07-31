using System.Threading.Tasks;
using Helpdesk.DomainModels.TicketLinks;
using Helpdesk.Services.TicketLinks.Results;

namespace Helpdesk.Services.Tickets.Commands.UnlinkTicket
{
    public interface IUnlinkTicketService
    {
        Task<UnlinkTicketsResult> Unlink(RemoveTicketsLink removeLink);
    }
}