using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Factories.UnassignTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Commands.UnassignTicket
{
    public class UnassignTicketService : IUnassignTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IUnassignTicketResultFactory _factory;

        public UnassignTicketService(
            IContextRepository<ITicketContext> repository,
            IUnassignTicketResultFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public virtual async Task<UnassignTicketResult> UnassignUser(int ticketId, Guid userGuid)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            ticket.UnassignUser();
            await _repository.SaveAsync();

            return _factory.Unassigned(ticketId, userGuid);
        }
    }
}