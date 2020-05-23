using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data.Common;
using Data.Repositories;
using Data.Specifications;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Queries.LookupTickets
{
    public class LookupTicketsService : AbstractTicketsLookup, ILookupTicketsService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IMapper _mapper;

        public LookupTicketsService(
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

            var details = _mapper.Map<IList<TicketLookup>>(tickets);

            return details;
        }

        public virtual async Task<PagedCollection<TicketLookup>> PagedLookup(int page, int pageSize, TicketLookupParams @params)
        {
            WithParameters(@params);

            (page, pageSize) = ValidatePaging(page, pageSize);

            var pagedCollection = await _repository.PagedListAsync(page, pageSize, _specification, ticket => ticket.TicketId);

            var details = _mapper.Map<IList<TicketLookup>>(pagedCollection.Items);

            return new PagedCollection<TicketLookup>
            (
                pagedCollection.Page,
                pagedCollection.PageSize,
                pagedCollection.TotalRecords,
                details
            );
        }

        private static LinqSpecification<Ticket> GetDefaultSpecification()
        {
            return new GetAllTickets().AsNoTracking();
        }
    }
}