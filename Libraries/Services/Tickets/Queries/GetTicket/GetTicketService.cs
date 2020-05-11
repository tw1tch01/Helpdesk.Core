using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Users.Specifications;

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

            var subQueries = new List<Task>();

            if (ticket.ClientId.HasValue) subQueries.Add(_repository.SingleAsync(new GetTicketById(ticketId)));
            if (ticket.ProjectId.HasValue) subQueries.Add(_repository.SingleAsync(new GetTicketById(ticketId)));
            if (ticket.ProjectId.HasValue) subQueries.Add(_repository.SingleAsync(new GetTicketById(ticketId)));

            if (ticket.ResolvedBy.HasValue) subQueries.Add(_repository.SingleAsync(new GetUserById(ticket.ResolvedBy.Value)));
            else if (ticket.ClosedBy.HasValue) subQueries.Add(_repository.SingleAsync(new GetUserById(ticket.ClosedBy.Value)));
            else if (ticket.ApprovedBy.HasValue) subQueries.Add(_repository.SingleAsync(new GetUserById(ticket.ApprovedBy.Value)));
            else if (ticket.ApprovalUserId.HasValue) subQueries.Add(_repository.SingleAsync(new GetUserById(ticket.ApprovalUserId.Value)));

            await Task.WhenAll(subQueries);

            var details = _mapper.Map<FullTicketDetails>(ticket);

            return details;
        }
    }
}