using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Factories.PauseTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Commands.PauseTicket
{
    public class PauseTicketService : IPauseTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IPauseTicketResultFactory _factory;

        public PauseTicketService(
            IContextRepository<ITicketContext> repository,
            IPauseTicketResultFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public virtual async Task<PauseTicketResult> Pause(int ticketId, Guid userGuid)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            switch (ticket.GetStatus())
            {
                case TicketStatus.Resolved:
                    return _factory.TicketAlreadyResolved(ticket);

                case TicketStatus.Closed:
                    return _factory.TicketAlreadyClosed(ticket);

                case TicketStatus.OnHold:
                    return _factory.TicketAlreadyPaused(ticket);
            }

            ticket.Pause(userGuid);
            await _repository.SaveAsync();

            return _factory.Paused(ticket);
        }
    }
}