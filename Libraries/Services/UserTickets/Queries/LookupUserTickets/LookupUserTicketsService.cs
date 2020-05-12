using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common;
using Helpdesk.Services.UserTickets.Specifications;

namespace Helpdesk.Services.UserTickets.Queries.LookupUserTickets
{
    public class LookupUserTicketsService : AbstractUserTicketsLookup, ILookupUserTicketsService, IPagedLookupUserTicketsService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IMapper _mapper;

        public LookupUserTicketsService(
            IContextRepository<ITicketContext> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<IList<TicketLookup>> Lookup(int userId)
        {
            _specification = new GetUserTicketsByUserId(userId);

            throw new NotImplementedException();
        }

        public Task<IList<TicketLookup>> PagedLookup(int userId, int page, int pageSize, TicketLookupParams @params)
        {
            throw new NotImplementedException();
        }
    }
}