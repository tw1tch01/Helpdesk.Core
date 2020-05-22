using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Tickets;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetResolvedTickets : LinqSpecification<Ticket>
    {
        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => ticket.ResolvedOn.HasValue;
        }
    }
}