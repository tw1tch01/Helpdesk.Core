using System.Threading.Tasks;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Tickets.Commands.UnlinkTicket
{
    public interface IUnlinkTicketService
    {
        Task<ProcessResult> Unlink(int fromTicketId, int toTicketId);
    }
}