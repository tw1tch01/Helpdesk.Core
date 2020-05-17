using System.Threading.Tasks;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Commands.ResolveTicket
{
    public interface IResolveTicketService
    {
        Task<ResolveTicketResult> Resolve(int ticketId, int userId);
    }
}