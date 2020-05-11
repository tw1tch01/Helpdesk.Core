using System.Threading.Tasks;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.UserTickets.Commands.UnassignTicket
{
    public interface IUnassignTicketService
    {
        Task<ProcessResult> Unassign(int ticketId, int userId);
    }
}