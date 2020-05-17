using System.Threading.Tasks;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Commands.StartTicket
{
    public interface IStartTicketService
    {
        Task<StartTicketResult> Start(int ticketId, int userId);
    }
}