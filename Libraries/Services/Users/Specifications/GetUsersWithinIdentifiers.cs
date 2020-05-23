using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Users;

namespace Helpdesk.Services.Users.Specifications
{
    public class GetUsersWithinIdentifiers : LinqSpecification<User>
    {
        private readonly IList<Guid> _identifiers;

        public GetUsersWithinIdentifiers(IList<Guid> identifiers)
        {
            _identifiers = identifiers;
        }

        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => _identifiers.Contains(user.Identifier);
        }
    }
}