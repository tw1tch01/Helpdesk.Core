using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.DomainModels.TicketLinks;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.TicketLinks.Factories.UnlinkTickets;
using Helpdesk.Services.TicketLinks.Results;
using Helpdesk.Services.TicketLinks.Specifications;

namespace Helpdesk.Services.TicketLinks.Commands.UnlinkTickets
{
    public class UnlinkTicketService : IUnlinkTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IUnlinkTicketsResultFactory _factory;

        public UnlinkTicketService(
            IContextRepository<ITicketContext> repository,
            IUnlinkTicketsResultFactory factory)
        {
            _repository = repository;
            _factory = factory;
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

            return _factory.Unlinked(unlink.FromTicketId, unlink.ToTicketId);
        }
    }
}