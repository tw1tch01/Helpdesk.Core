using System.Threading.Tasks;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Tickets.Commands.ApproveTicket
{
    public interface IApproveTicketService
    {
        Task<ProcessResult> Approve(int ticketId, int userId);
    }
}