using System.Threading.Tasks;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Tickets.Commands.UpdateTicket
{
    public interface IUpdateTicketService
    {
        Task<ProcessResult> Update(int ticketId, UpdateTicketDto ticketDto);
    }
}