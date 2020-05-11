using System.Threading.Tasks;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Tickets.Commands.DeleteTicket
{
    public interface IDeleteTicketService
    {
        Task<ProcessResult> Delete(int ticketId);
    }
}