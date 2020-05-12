using System.Threading.Tasks;
using Data.Common;
using Helpdesk.DomainModels.Tickets;

namespace Helpdesk.Services.Tickets.Queries.LookupTickets
{
    public interface IPagedLookupTicketsService
    {
        Task<PagedCollection<TicketLookup>> PagedLookup(int page, int pageSize, TicketLookupParams @params);
    }
}