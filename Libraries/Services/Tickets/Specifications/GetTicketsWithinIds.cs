using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Tickets;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetTicketsWithinIds : LinqSpecification<Ticket>
    {
        private readonly IList<int> _ticketIds;

        public GetTicketsWithinIds(IList<int> ticketIds)
        {
            _ticketIds = ticketIds;
        }

        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => _ticketIds.Contains(ticket.TicketId);
        }
    }
}