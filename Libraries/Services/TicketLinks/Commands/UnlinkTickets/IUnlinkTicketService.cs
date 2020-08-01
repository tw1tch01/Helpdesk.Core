using System.Threading.Tasks;
using Helpdesk.DomainModels.TicketLinks;
using Helpdesk.Services.TicketLinks.Results;

namespace Helpdesk.Services.TicketLinks.Commands.UnlinkTickets
{
    public interface IUnlinkTicketService
    {
        Task<UnlinkTicketsResult> Unlink(UnlinkTicket unlink);
    }
}