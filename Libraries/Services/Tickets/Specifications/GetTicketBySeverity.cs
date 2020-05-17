using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetTicketBySeverity : LinqSpecification<Ticket>
    {
        private readonly Severity _severity;

        public GetTicketBySeverity(Severity severity)
        {
            _severity = severity;
        }

        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => ticket.Severity == _severity;
        }
    }
}