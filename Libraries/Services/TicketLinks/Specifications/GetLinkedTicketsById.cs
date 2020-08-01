using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.TicketLinks.Specifications
{
    public class GetLinkedTicketsById : LinqSpecification<TicketLink>
    {
        public GetLinkedTicketsById(int fromTicketId, int toTicketId)
        {
            FromTicketId = fromTicketId;
            ToTicketId = toTicketId;
        }

        public int FromTicketId { get; }
        public int ToTicketId { get; }

        public override Expression<Func<TicketLink, bool>> AsExpression()
        {
            return ticketLink => ticketLink.FromTicketId == FromTicketId &&
                                 ticketLink.ToTicketId == ToTicketId;
        }
    }
}