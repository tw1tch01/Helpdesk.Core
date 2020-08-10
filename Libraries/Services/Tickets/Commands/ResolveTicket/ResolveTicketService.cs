using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Domain.Tickets.Events;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Factories.ResolveTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Commands.ResolveTicket
{
    public class ResolveTicketService : IResolveTicketService
    {
        private readonly IEntityRepository<ITicketContext> _repository;
        private readonly IResolveTicketResultFactory _factory;
        private readonly IEventService _eventService;

        public ResolveTicketService(
            IEntityRepository<ITicketContext> repository,
            IResolveTicketResultFactory factory,
            IEventService eventService)
        {
            _repository = repository;
            _factory = factory;
            _eventService = eventService;
        }

        public virtual async Task<ResolveTicketResult> Resolve(int ticketId, Guid userGuid)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            switch (ticket.GetStatus())
            {
                case TicketStatus.Resolved:
                    return _factory.TicketAlreadyResolved(ticket);

                case TicketStatus.Closed:
                    return _factory.TicketAlreadyClosed(ticket);
            }

            ticket.Resolve(userGuid);
            await _repository.SaveAsync();

            await _eventService.Publish(new TicketResolvedEvent(ticketId, userGuid));

            return _factory.Resolved(ticket);
        }
    }
}