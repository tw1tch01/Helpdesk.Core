using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetTicketById : LinqSpecification<Ticket>
    {
        private readonly int _ticketId;

        public GetTicketById(int ticketId)
        {
            _ticketId = ticketId;
        }

        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => ticket.TicketId == _ticketId;
        }
    }
}