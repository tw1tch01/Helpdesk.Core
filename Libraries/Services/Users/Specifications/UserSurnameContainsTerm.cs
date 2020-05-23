using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Users;

namespace Helpdesk.Services.Users.Specifications
{
    public class UserSurnameContainsTerm : LinqSpecification<User>
    {
        internal readonly string _term;

        public UserSurnameContainsTerm(string term)
        {
            _term = term;
        }

        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => user.Surname.Contains(_term);
        }
    }
}