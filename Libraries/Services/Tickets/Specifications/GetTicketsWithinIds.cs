using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Tickets;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetTicketsWithinIds : LinqSpecification<Ticket>
    {
        public GetTicketsWithinIds(IList<int> ticketIds)
        {
            TicketIds = ticketIds;
        }

        public IList<int> TicketIds { get; }

        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => TicketIds.Contains(ticket.TicketId);
        }
    }
}