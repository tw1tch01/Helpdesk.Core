using System.Collections.Generic;
using System.Threading.Tasks;
using Helpdesk.DomainModels.Tickets;

namespace Helpdesk.Services.Tickets.Queries.LookupTickets
{
    public interface ILookupTicketsService
    {
        Task<IList<TicketLookup>> Lookup(TicketLookupParams @params);
    }
}