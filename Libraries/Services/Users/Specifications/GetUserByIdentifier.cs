using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Users;

namespace Helpdesk.Services.Users.Specifications
{
    public class GetUserByIdentifier : LinqSpecification<User>
    {
        internal readonly Guid _identifier;

        public GetUserByIdentifier(Guid identifier)
        {
            _identifier = identifier;
        }

        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => user.Identifier == _identifier;
        }
    }
}