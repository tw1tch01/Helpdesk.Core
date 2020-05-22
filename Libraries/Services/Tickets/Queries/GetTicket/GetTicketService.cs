using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Queries.GetTicket
{
    public class GetTicketService : IGetTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IMapper _mapper;

        public GetTicketService(
            IContextRepository<ITicketContext> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<FullTicketDetails> Get(int ticketId)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return null;

            var details = _mapper.Map<FullTicketDetails>(ticket);

            return details;
        }
    }
}