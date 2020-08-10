using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Events;
using Helpdesk.DomainModels.TicketLinks;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.TicketLinks.Factories.LinkTickets;
using Helpdesk.Services.TicketLinks.Results;
using Helpdesk.Services.TicketLinks.Specifications;

namespace Helpdesk.Services.TicketLinks.Commands.LinkTickets
{
    public class LinkTicketService : ILinkTicketService
    {
        private readonly IEntityRepository<ITicketContext> _repository;
        private readonly IMapper _mapper;
        private readonly ILinkTicketsResultFactory _factory;
        private readonly IEventService _eventService;

        public LinkTicketService(
            IEntityRepository<ITicketContext> repository,
            IMapper mapper,
            ILinkTicketsResultFactory factory,
            IEventService eventService)
        {
            _repository = repository;
            _mapper = mapper;
            _factory = factory;
            _eventService = eventService;
        }

        public virtual async Task<LinkTicketsResult> Link(LinkTicket newLink)
        {
            if (newLink.IsSelfLink()) throw new ArgumentException("Cannot link a ticket to itself.");

            var existingTicketLinkSpec = new GetLinkedTicketsById(newLink.FromTicketId, newLink.ToTicketId)
                                       | new GetLinkedTicketsById(newLink.ToTicketId, newLink.FromTicketId);

            var existingTicketLink = await _repository.SingleAsync(existingTicketLinkSpec.AsNoTracking());

            if (existingTicketLink != null) return _factory.TicketsAlreadyLinked(existingTicketLink);

            var ticketLink = _mapper.Map<TicketLink>(newLink);

            await _repository.AddAsync(ticketLink);
            await _repository.SaveAsync();

            await _eventService.Publish(new TicketsLinkedEvent(newLink.FromTicketId, newLink.ToTicketId, newLink.LinkType));

            return _factory.Linked(ticketLink);
        }
    }
}