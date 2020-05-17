using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class TicketNameContainsTerm : LinqSpecification<Ticket>
    {
        private readonly string _term;

        public TicketNameContainsTerm(string term)
        {
            _term = term;
        }

        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => ticket.Name.Contains(_term);
        }
    }
}