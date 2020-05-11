using System.Threading.Tasks;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Tickets.Commands.CloseTicket
{
    public interface ICloseTicketService
    {
        Task<ProcessResult> Close(int ticketId, int userId);
    }
}