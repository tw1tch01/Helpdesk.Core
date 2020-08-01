using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Tickets;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class TicketNameContainsTerm : LinqSpecification<Ticket>
    {
        public TicketNameContainsTerm(string term)
        {
            Term = term;
        }

        public string Term { get; }

        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => ticket.Name.Contains(Term);
        }
    }
}