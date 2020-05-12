using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetTicketWithinIds : LinqSpecification<Ticket>
    {
        private readonly IList<int> _ticketIds;

        public GetTicketWithinIds(IList<int> ticketIds)
        {
            _ticketIds = ticketIds;
        }

        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => _ticketIds.Contains(ticket.TicketId);
        }
    }
}