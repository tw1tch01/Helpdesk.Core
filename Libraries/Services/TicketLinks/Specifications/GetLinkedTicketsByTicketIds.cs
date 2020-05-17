using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.TicketLinks.Specifications
{
    public class GetLinkedTicketsByTicketIds : LinqSpecification<TicketLink>
    {
        private readonly IList<int> _ticketIds;

        public GetLinkedTicketsByTicketIds(IList<int> ticketIds)
        {
            _ticketIds = ticketIds;
        }

        public override Expression<Func<TicketLink, bool>> AsExpression()
        {
            return ticketLink => _ticketIds.Contains(ticketLink.FromTicketId) || _ticketIds.Contains(ticketLink.ToTicketId);
        }
    }
}