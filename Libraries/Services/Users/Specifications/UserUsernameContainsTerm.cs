using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Users;

namespace Helpdesk.Services.Users.Specifications
{
    public class UserUsernameContainsTerm : LinqSpecification<User>
    {
        internal readonly string _term;

        public UserUsernameContainsTerm(string term)
        {
            _term = term;
        }

        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => user.Username.Contains(_term);
        }
    }
}