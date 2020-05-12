using System.Collections.Generic;
using System.Threading.Tasks;
using Helpdesk.DomainModels.Tickets;

namespace Helpdesk.Services.UserTickets.Queries.LookupUserTickets
{
    public interface ILookupUserTicketsService
    {
        Task<IList<TicketLookup>> Lookup(int userId);
    }
}