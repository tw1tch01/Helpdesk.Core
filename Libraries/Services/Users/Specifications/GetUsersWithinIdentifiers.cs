using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Users;

namespace Helpdesk.Services.Users.Specifications
{
    public class GetUsersWithinIdentifiers : LinqSpecification<User>
    {
        public GetUsersWithinIdentifiers(IList<Guid> identifiers)
        {
            Identifiers = identifiers;
        }

        public IList<Guid> Identifiers { get; }

        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => Identifiers.Contains(user.Identifier);
        }
    }
}