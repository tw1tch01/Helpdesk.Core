using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Data.Specifications;
using Helpdesk.Domain.Entities;

[assembly: InternalsVisibleTo("Helpdesk.Services.UnitTests")]

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetTicketById : LinqSpecification<Ticket>
    {
        internal readonly int _ticketId;

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