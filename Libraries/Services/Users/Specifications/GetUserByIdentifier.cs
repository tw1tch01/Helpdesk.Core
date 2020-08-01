using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Users;

namespace Helpdesk.Services.Users.Specifications
{
    public class GetUserByIdentifier : LinqSpecification<User>
    {
        public GetUserByIdentifier(Guid identifier)
        {
            Identifier = identifier;
        }

        public Guid Identifier { get; }

        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => user.Identifier == Identifier;
        }
    }
}