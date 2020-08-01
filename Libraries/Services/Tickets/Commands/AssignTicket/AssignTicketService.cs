using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Factories.AssignTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Commands.AssignTicket
{
    public class AssignTicketService : IAssignTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IAssignTicketResultFactory _factory;

        public AssignTicketService(
            IContextRepository<ITicketContext> repository,
            IAssignTicketResultFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public virtual async Task<AssignTicketResult> AssignUser(int ticketId, Guid userGuid)
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

            ticket.AssignUser(userGuid);
            await _repository.SaveAsync();

            return _factory.Assigned(ticket);
        }
    }
}