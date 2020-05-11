using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.UserTickets.Specifications
{
    public class GetUserTicketById : LinqSpecification<UserTicket>
    {
        private readonly int _ticketId;
        private readonly int _userId;

        public GetUserTicketById(int ticketId, int userId)
        {
            _ticketId = ticketId;
            _userId = userId;
        }

        public override Expression<Func<UserTicket, bool>> AsExpression()
        {
            return userTicket => userTicket.TicketId == _ticketId && userTicket.UserId == _userId;
        }
    }
}