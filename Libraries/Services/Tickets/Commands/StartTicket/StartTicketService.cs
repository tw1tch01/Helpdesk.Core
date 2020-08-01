﻿using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Factories.StartTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Commands.StartTicket
{
    public class StartTicketService : IStartTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IStartTicketResultFactory _factory;

        public StartTicketService(
            IContextRepository<ITicketContext> repository,
            IStartTicketResultFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public virtual async Task<StartTicketResult> Start(int ticketId, Guid userGuid)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            switch (ticket.GetStatus())
            {
                case TicketStatus.Resolved:
                    return _factory.TicketAlreadyResolved(ticket);

                case TicketStatus.Closed:
                    return _factory.TicketAlreadyClosed(ticket);

                case TicketStatus.InProgress:
                    return _factory.TicketAlreadyStarted(ticket);
            }

            ticket.Start(userGuid);
            await _repository.SaveAsync();

            return _factory.Started(ticket);
        }
    }
}