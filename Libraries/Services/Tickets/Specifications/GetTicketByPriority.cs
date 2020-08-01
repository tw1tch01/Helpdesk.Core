using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Enums;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetTicketByPriority : LinqSpecification<Ticket>
    {
        public GetTicketByPriority(Priority priority)
        {
            Priority = priority;
        }

        public Priority Priority { get; }

        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => ticket.Priority == Priority;
        }
    }
}