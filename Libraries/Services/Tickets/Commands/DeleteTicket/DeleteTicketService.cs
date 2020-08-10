using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Tickets.Events;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Factories.DeleteTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Commands.DeleteTicket
{
    public class DeleteTicketService : IDeleteTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IDeleteTicketResultFactory _factory;
        private readonly IEventService _eventService;

        public DeleteTicketService(
            IContextRepository<ITicketContext> repository,
            IDeleteTicketResultFactory factory,
            IEventService eventService)
        {
            _repository = repository;
            _factory = factory;
            _eventService = eventService;
        }

        public virtual async Task<DeleteTicketResult> Delete(int ticketId, Guid userGuid)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            _repository.Remove(ticket);
            await _repository.SaveAsync();

            await _eventService.Publish(new TicketDeletedEvent(ticketId, userGuid));

            return _factory.Deleted(ticketId, userGuid);
        }
    }
}