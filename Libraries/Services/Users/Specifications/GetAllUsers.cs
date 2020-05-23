using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Users;

namespace Helpdesk.Services.Users.Specifications
{
    public class GetAllUsers : LinqSpecification<User>
    {
        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => true;
        }
    }
}