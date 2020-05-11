using System.Threading.Tasks;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Tickets.Commands.RequestApproval
{
    public interface IRequestTicketApprovalService
    {
        Task<ProcessResult> RequestApproval(int ticketId, int userId);
    }
}