using System.Threading.Tasks;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Commands.OpenTicket
{
    public interface IOpenTicketService
    {
        Task<OpenTicketResult> Open(OpenTicketDto ticketDto);
    }
}