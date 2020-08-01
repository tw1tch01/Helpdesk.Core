using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Users;

namespace Helpdesk.Services.Users.Specifications
{
    public class GetUserById : LinqSpecification<User>
    {
        public GetUserById(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; }

        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => user.UserId == UserId;
        }
    }
}