using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Tickets.Events;
using Helpdesk.Services.Common;
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
        private readonly IEventService _eventService;

        public UnassignTicketService(
            IContextRepository<ITicketContext> repository,
            IUnassignTicketResultFactory factory,
            IEventService eventService)
        {
            _repository = repository;
            _factory = factory;
            _eventService = eventService;
        }

        public virtual async Task<UnassignTicketResult> UnassignUser(int ticketId, Guid userGuid)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            ticket.UnassignUser();
            await _repository.SaveAsync();

            await _eventService.Publish(new TicketUnassignedEvent(ticketId, userGuid));

            return _factory.Unassigned(ticketId, userGuid);
        }
    }
}