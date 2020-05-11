using System.Threading.Tasks;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Tickets.Commands.OpenTicket
{
    public interface IOpenTicketService
    {
        Task<ProcessResult> Open(OpenTicketDto ticketDto);
    }
}