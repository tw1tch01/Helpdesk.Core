using System.Threading.Tasks;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Commands.UpdateTicket
{
    public interface IUpdateTicketService
    {
        Task<UpdateTicketResult> Update(int ticketId, EditTicket editTicket);
    }
}