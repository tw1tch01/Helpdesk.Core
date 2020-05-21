using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data.Common;
using Data.Repositories;
using Data.Specifications;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Queries.LookupTickets
{
    public class LookupTicketsService : AbstractTicketsLookup, ILookupTicketsService
    {
        private const int _defaultPageSize = 25;
        private const int _maximumPageSize = 50;
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
            return new GetAllTickets();
        }
    }
}