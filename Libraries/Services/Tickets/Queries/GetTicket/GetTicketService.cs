using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Clients.Specifications;
using Helpdesk.Services.Common;
using Helpdesk.Services.Projects.Specifications;
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

            await _repository.GetAsync(new GetClientById(ticket.ClientId).Include(c => c.Organization));
            if (ticket.ProjectId.HasValue) await _repository.GetAsync(new GetProjectById(ticket.ProjectId.Value));

            //if (ticket.ClientId.HasValue) await _repository.SingleAsync(new GetTicketById(ticketId)));
            //if (ticket.ProjectId.HasValue) await _repository.SingleAsync(new GetTicketById(ticketId)));
            //if (ticket.ProjectId.HasValue) await _repository.SingleAsync(new GetTicketById(ticketId)));

            if (ticket.ResolvedBy.HasValue) await _repository.SingleAsync(new GetUserById(ticket.ResolvedBy.Value));
            else if (ticket.ClosedBy.HasValue) await _repository.SingleAsync(new GetUserById(ticket.ClosedBy.Value));
            else if (ticket.ApprovedBy.HasValue) await _repository.SingleAsync(new GetUserById(ticket.ApprovedBy.Value));
            else if (ticket.ApprovalUserId.HasValue) await _repository.SingleAsync(new GetUserById(ticket.ApprovalUserId.Value));

            var details = _mapper.Map<FullTicketDetails>(ticket);

            return details;
        }
    }
}