using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Enums;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetTicketByPriority : LinqSpecification<Ticket>
    {
        private readonly Priority _priority;

        public GetTicketByPriority(Priority priority)
        {
            _priority = priority;
        }

        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => ticket.Priority == _priority;
        }
    }
}