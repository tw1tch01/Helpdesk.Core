using System;
using System.Threading.Tasks;
using Data.Repositories;
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

        public DeleteTicketService(
            IContextRepository<ITicketContext> repository,
            IDeleteTicketResultFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public virtual async Task<DeleteTicketResult> Delete(int ticketId, Guid userGuid)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            _repository.Remove(ticket);
            await _repository.SaveAsync();

            return _factory.Deleted(ticketId, userGuid);
        }
    }
}