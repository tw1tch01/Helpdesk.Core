using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Tickets;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetTicketById : LinqSpecification<Ticket>
    {
        public GetTicketById(int ticketId)
        {
            TicketId = ticketId;
        }

        public int TicketId { get; }

        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            return ticket => ticket.TicketId == TicketId;
        }
    }
}