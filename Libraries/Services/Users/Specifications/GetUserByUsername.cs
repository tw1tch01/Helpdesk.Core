using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Data.Specifications;
using Helpdesk.Domain.Users;

[assembly: InternalsVisibleTo("Helpdesk.Services.UnitTests")]

namespace Helpdesk.Services.Users.Specifications
{
    public class GetUserByUsername : LinqSpecification<User>
    {
        internal readonly string _username;

        public GetUserByUsername(string username)
        {
            _username = username;
        }

        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => user.Username == _username;
        }
    }
}