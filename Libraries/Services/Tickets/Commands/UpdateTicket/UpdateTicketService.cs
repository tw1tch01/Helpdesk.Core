using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using FluentValidation;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Factories.UpdateTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Commands.UpdateTicket
{
    public class UpdateTicketService : IUpdateTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IMapper _mapper;
        private readonly IUpdateTicketResultFactory _factory;
        private readonly IValidator<EditTicket> _validator;

        public UpdateTicketService(
            IContextRepository<ITicketContext> repository,
            IMapper mapper,
            IUpdateTicketResultFactory factory,
            IValidator<EditTicket> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _factory = factory;
            _validator = validator;
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

            return _factory.Updated(ticket, changes);
        }
    }
}