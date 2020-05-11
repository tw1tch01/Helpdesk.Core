using System.Threading.Tasks;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Tickets.Commands.PauseTicket
{
    public interface IPauseTicketService
    {
        Task<ProcessResult> Pause(int ticketId);
    }
}