using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Users;

namespace Helpdesk.Services.Users.Specifications
{
    public class UserUsernameContainsTerm : LinqSpecification<User>
    {
        public UserUsernameContainsTerm(string term)
        {
            Term = term;
        }

        public string Term { get; }

        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => user.Username.Contains(Term);
        }
    }
}