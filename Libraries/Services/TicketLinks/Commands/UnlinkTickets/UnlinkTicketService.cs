using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Tickets.Events;
using Helpdesk.DomainModels.TicketLinks;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.TicketLinks.Factories.UnlinkTickets;
using Helpdesk.Services.TicketLinks.Results;
using Helpdesk.Services.TicketLinks.Specifications;

namespace Helpdesk.Services.TicketLinks.Commands.UnlinkTickets
{
    public class UnlinkTicketService : IUnlinkTicketService
    {
        private readonly IEntityRepository<ITicketContext> _repository;
        private readonly IUnlinkTicketsResultFactory _factory;
        private readonly IEventService _eventService;

        public UnlinkTicketService(
            IEntityRepository<ITicketContext> repository,
            IUnlinkTicketsResultFactory factory,
            IEventService eventService)
        {
            _repository = repository;
            _factory = factory;
            _eventService = eventService;
        }

        public virtual async Task<UnlinkTicketsResult> Unlink(UnlinkTicket unlink)
        {
            if (unlink.IsSelfUnlink()) throw new ArgumentException("Cannot unlink a ticket from itself.");

            var ticketLinkSpec = new GetLinkedTicketsById(unlink.FromTicketId, unlink.ToTicketId)
                               | new GetLinkedTicketsById(unlink.ToTicketId, unlink.FromTicketId);

            var ticketLink = await _repository.SingleAsync(ticketLinkSpec);

            if (ticketLink == null) return _factory.TicketsNotLinked(unlink.FromTicketId, unlink.ToTicketId);

            _repository.Remove(ticketLink);
            await _repository.SaveAsync();

            await _eventService.Publish(new TicketsUnlinkedEvent(unlink.FromTicketId, unlink.ToTicketId));

            return _factory.Unlinked(unlink.FromTicketId, unlink.ToTicketId);
        }
    }
}