using System.Threading.Tasks;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Commands.DeleteTicket
{
    public interface IDeleteTicketService
    {
        Task<DeleteTicketResult> Delete(int ticketId, int userId);
    }
}