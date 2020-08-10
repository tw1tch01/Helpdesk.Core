using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using FluentValidation;
using Helpdesk.Domain.Tickets.Events;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Factories.UpdateTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Commands.UpdateTicket
{
    public class UpdateTicketService : IUpdateTicketService
    {
        private readonly IEntityRepository<ITicketContext> _repository;
        private readonly IMapper _mapper;
        private readonly IUpdateTicketResultFactory _factory;
        private readonly IValidator<EditTicket> _validator;
        private readonly IEventService _eventService;

        public UpdateTicketService(
            IEntityRepository<ITicketContext> repository,
            IMapper mapper,
            IUpdateTicketResultFactory factory,
            IValidator<EditTicket> validator,
            IEventService eventService)
        {
            _repository = repository;
            _mapper = mapper;
            _factory = factory;
            _validator = validator;
            _eventService = eventService;
        }

        public virtual async Task<UpdateTicketResult> Update(int ticketId, EditTicket editTicket)
        {
            if (editTicket == null) throw new ArgumentNullException(nameof(editTicket));

            var validationResult = await _validator.ValidateAsync(editTicket);

            if (!validationResult.IsValid) return _factory.ValidationFailure(ticketId, validationResult.Errors);

            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            var changes = editTicket.GetChanges(ticket);

            _mapper.Map(editTicket, ticket);
            await _repository.SaveAsync();

            await _eventService.Publish(new TicketUpdatedEvent(ticketId, changes));

            return _factory.Updated(ticket, changes);
        }
    }
}