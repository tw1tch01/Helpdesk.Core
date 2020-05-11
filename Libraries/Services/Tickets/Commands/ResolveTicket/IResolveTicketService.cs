using System.Threading.Tasks;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Tickets.Commands.ResolveTicket
{
    public interface IResolveTicketService
    {
        Task<ProcessResult> Resolve(int ticketId, int userId);
    }
}