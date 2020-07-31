using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.TicketLinks.Specifications
{
    public class GetLinkedTicketsById : LinqSpecification<TicketLink>
    {
        private readonly int _fromTicketId;
        private readonly int _toTicketId;

        public GetLinkedTicketsById(int fromTicketId, int toTicketId)
        {
            _fromTicketId = fromTicketId;
            _toTicketId = toTicketId;
        }

        public override Expression<Func<TicketLink, bool>> AsExpression()
        {
            return ticketLink => ticketLink.FromTicketId == _fromTicketId && ticketLink.ToTicketId == _toTicketId;
        }
    }
}