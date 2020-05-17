using System.Threading.Tasks;
using Helpdesk.DomainModels.Tickets;

namespace Helpdesk.Services.Tickets.Queries.GetTicket
{
    public interface IGetTicketService
    {
        Task<FullTicketDetails> Get(int ticketId);
    }
}