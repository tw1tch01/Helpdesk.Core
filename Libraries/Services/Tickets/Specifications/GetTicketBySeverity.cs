using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Enums;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetTicketBySeverity : LinqSpecification<Ticket>
    {
        public GetTicketBySeverity(Severity severity)
        {
            Severity = severity;
        }

        public Severity Severity { get; }

        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => ticket.Severity == Severity;
        }
    }
}