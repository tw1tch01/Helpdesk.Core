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
        public GetUserByUsername(string username)
        {
            Username = username;
        }

        public string Username { get; }

        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => user.Username == Username;
        }
    }
}