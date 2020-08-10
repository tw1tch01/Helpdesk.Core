using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using FluentValidation;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Events;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Factories.OpenTicket;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Commands.OpenTicket
{
    public class OpenTicketService : IOpenTicketService
    {
        private readonly IEntityRepository<ITicketContext> _repository;
        private readonly IMapper _mapper;
        private readonly IOpenTicketResultFactory _factory;
        private readonly IValidator<NewTicket> _validator;
        private readonly IEventService _eventService;

        public OpenTicketService(
            IEntityRepository<ITicketContext> repository,
            IMapper mapper,
            IOpenTicketResultFactory factory,
            IValidator<NewTicket> validator,
            IEventService eventService)
        {
            _repository = repository;
            _mapper = mapper;
            _factory = factory;
            _validator = validator;
            _eventService = eventService;
        }

        public virtual async Task<OpenTicketResult> Open(NewTicket newTicket)
        {
            if (newTicket == null) throw new ArgumentNullException(nameof(newTicket));

            var validationResult = await _validator.ValidateAsync(newTicket);

            if (!validationResult.IsValid) return _factory.ValidationFailure(validationResult.Errors);

            var ticket = _mapper.Map<Ticket>(newTicket);

            await _repository.AddAsync(ticket);
            await _repository.SaveAsync();

            await _eventService.Publish(new TicketOpenedEvent(ticket.TicketId, ticket.UserGuid));

            return _factory.Opened(ticket);
        }
    }
}