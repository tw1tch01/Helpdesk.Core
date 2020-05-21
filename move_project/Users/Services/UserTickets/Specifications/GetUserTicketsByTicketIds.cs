using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.UserTickets.Specifications
{
    public class GetUserTicketsByTicketIds : LinqSpecification<UserTicket>
    {
        private readonly IList<int> _ticketIds;

        public GetUserTicketsByTicketIds(IList<int> ticketIds)
        {
            _ticketIds = ticketIds;
        }

        public override Expression<Func<UserTicket, bool>> AsExpression()
        {
            return userTicket => _ticketIds.Contains(userTicket.TicketId);
        }
    }
}