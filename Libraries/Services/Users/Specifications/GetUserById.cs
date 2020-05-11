using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.Users.Specifications
{
    public class GetUserById : LinqSpecification<User>
    {
        private readonly int _userId;

        public GetUserById(int userId)
        {
            _userId = userId;
        }

        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => user.UserId == _userId;
        }
    }
}