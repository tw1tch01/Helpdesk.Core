using System.Threading.Tasks;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Tickets.Commands.ReopenTicket
{
    public interface IReopenTicketService
    {
        Task<ProcessResult> Reopen(int ticketId);
    }
}