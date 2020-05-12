using System.Collections.Generic;
using System.Threading.Tasks;
using Helpdesk.DomainModels.Tickets;

namespace Helpdesk.Services.UserTickets.Queries.LookupUserTickets
{
    public interface IPagedLookupUserTicketsService
    {
        Task<IList<TicketLookup>> PagedLookup(int userId, int page, int pageSize, TicketLookupParams @params);
    }
}