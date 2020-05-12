using System.Threading.Tasks;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Commands.CloseTicket
{
    public interface ICloseTicketService
    {
        Task<CloseTicketResult> Close(int ticketId, int userId);
    }
}