using System.Threading.Tasks;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Tickets.Commands.StartTicket
{
    public interface IStartTicketService
    {
        Task<ProcessResult> Start(int ticketId);
    }
}