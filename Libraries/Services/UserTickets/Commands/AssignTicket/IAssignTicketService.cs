using System.Threading.Tasks;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Tickets.Commands.AssignTicket
{
    public interface IAssignTicketService
    {
        Task<ProcessResult> Assign(int ticketId, int userId);
    }
}