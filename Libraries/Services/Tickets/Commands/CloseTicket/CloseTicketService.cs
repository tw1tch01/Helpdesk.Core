using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Factories.CloseTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Commands.CloseTicket
{
    public class CloseTicketService : ICloseTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly ICloseTicketResultFactory _factory;

        public CloseTicketService(
            IContextRepository<ITicketContext> repository,
            ICloseTicketResultFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public virtual async Task<CloseTicketResult> Close(int ticketId, Guid userGuid)
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

            ticket.Close(userGuid);
            await _repository.SaveAsync();

            return _factory.Closed(ticket);
        }
    }
}