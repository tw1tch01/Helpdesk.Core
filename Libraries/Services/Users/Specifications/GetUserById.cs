using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Data.Specifications;
using Helpdesk.Domain.Entities;

[assembly: InternalsVisibleTo("Helpdesk.Services.UnitTests")]

namespace Helpdesk.Services.Users.Specifications
{
    public class GetUserById : LinqSpecification<User>
    {
        internal readonly int _userId;

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