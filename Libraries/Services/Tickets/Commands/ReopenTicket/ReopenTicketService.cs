using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Tickets.Events;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Factories.ReopenTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Commands.ReopenTicket
{
    public class ReopenTicketService : IReopenTicketService
    {
        private readonly IEntityRepository<ITicketContext> _repository;
        private readonly IReopenTicketResultFactory _factory;
        private readonly IEventService _eventService;

        public ReopenTicketService(
            IEntityRepository<ITicketContext> repository,
            IReopenTicketResultFactory factory,
            IEventService eventService)
        {
            _repository = repository;
            _factory = factory;
            _eventService = eventService;
        }

        public virtual async Task<ReopenTicketResult> Reopen(int ticketId, Guid userGuid)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            ticket.Reopen();
            await _repository.SaveAsync();

            await _eventService.Publish(new TicketReopenedEvent(ticketId, userGuid));

            return _factory.Reopened(ticket, userGuid);
        }
    }
}