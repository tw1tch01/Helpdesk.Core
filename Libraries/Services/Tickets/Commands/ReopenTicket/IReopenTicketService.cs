using System.Threading.Tasks;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Commands.ReopenTicket
{
    public interface IReopenTicketService
    {
        Task<ReopenTicketResult> Reopen(int ticketId);
    }
}