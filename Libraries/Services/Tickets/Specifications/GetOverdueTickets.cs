using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Tickets;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetOverdueTickets : LinqSpecification<Ticket>
    {
        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => !ticket.ResolvedOn.HasValue
                          && !ticket.ClosedOn.HasValue
                          && !ticket.PausedOn.HasValue
                          && !ticket.StartedOn.HasValue
                          && ticket.DueDate < DateTimeOffset.UtcNow;
        }
    }
}