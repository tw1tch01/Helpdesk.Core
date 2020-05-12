using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.UserTickets.Specifications
{
    public class GetUserTicketsByUserId : LinqSpecification<UserTicket>
    {
        private readonly int _userId;

        public GetUserTicketsByUserId(int userId)
        {
            _userId = userId;
        }

        public override Expression<Func<UserTicket, bool>> AsExpression()
        {
            return userTicket => userTicket.UserId == _userId;
        }
    }
}