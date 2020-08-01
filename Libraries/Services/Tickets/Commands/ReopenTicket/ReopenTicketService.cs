using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Factories.ReopenTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Commands.ReopenTicket
{
    public class ReopenTicketService : IReopenTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IReopenTicketResultFactory _factory;

        public ReopenTicketService(
            IContextRepository<ITicketContext> repository,
            IReopenTicketResultFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public virtual async Task<ReopenTicketResult> Reopen(int ticketId, Guid userGuid)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            ticket.Reopen();
            await _repository.SaveAsync();

            return _factory.Reopened(ticket, userGuid);
        }
    }
}