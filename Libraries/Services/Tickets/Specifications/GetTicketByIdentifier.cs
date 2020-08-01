using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Tickets;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetTicketByIdentifier : LinqSpecification<Ticket>
    {
        public GetTicketByIdentifier(Guid identifier)
        {
            Identifier = identifier;
        }

        public Guid Identifier { get; }

        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => ticket.Identifier == Identifier;
        }
    }
}