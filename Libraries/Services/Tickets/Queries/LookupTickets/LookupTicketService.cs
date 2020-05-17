using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Common;
using Data.Repositories;
using Data.Specifications;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common;
using Helpdesk.Services.TicketLinks.Specifications;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.UserTickets.Specifications;

namespace Helpdesk.Services.Tickets.Queries.LookupTickets
{
    public class LookupTicketService : AbstractTicketsLookup, ILookupTicketsService
    {
        private const int _defaultPageSize = 25;
        private const int _maximumPageSize = 50;
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IMapper _mapper;

        public LookupTicketService(
            IContextRepository<ITicketContext> repository,
            IMapper mapper)
            : base(GetDefaultSpecification())
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<IList<TicketLookup>> Lookup(TicketLookupParams @params)
        {
            WithParameters(@params);

            var tickets = await _repository.ListAsync(_specification);

            if (tickets.Any()) await ExecuteSubQueries(tickets.Select(t => t.TicketId).ToList());

            var details = _mapper.Map<IList<TicketLookup>>(tickets);

            return details;
        }

        public virtual async Task<PagedCollection<TicketLookup>> PagedLookup(int page, int pageSize, TicketLookupParams @params)
        {
            WithParameters(@params);

            (page, pageSize) = ValidatePaging(page, pageSize);

            var pagedCollection = await _repository.PagedListAsync(page, pageSize, _specification, ticket => ticket.TicketId);

            if (pagedCollection.Items.Any()) await ExecuteSubQueries(pagedCollection.Items.Select(t => t.TicketId).ToList());

            var details = _mapper.Map<ICollection<TicketLookup>>(pagedCollection.Items);

            return new PagedCollection<TicketLookup>
            (
                pagedCollection.Page,
                pagedCollection.PageSize,
                pagedCollection.TotalRecords,
                details
            );
        }

        internal static (int page, int pageSize) ValidatePaging(int page, int pageSize)
        {
            if (page < 0) page = 0;

            if (pageSize <= 0) pageSize = _defaultPageSize;
            else if (pageSize > _maximumPageSize) pageSize = _maximumPageSize;

            return (page, pageSize);
        }

        private static LinqSpecification<Ticket> GetDefaultSpecification()
        {
            return new GetAllTickets()
                .Include(t => t.Client)
                .Include(t => t.Client.Organization)
                .Include(t => t.Project);
        }

        private async Task ExecuteSubQueries(IList<int> ticketIds)
        {
            await _repository.ListAsync(new GetUserTicketsByTicketIds(ticketIds));
            await _repository.ListAsync(new GetLinkedTicketsByTicketIds(ticketIds));
        }
    }
}