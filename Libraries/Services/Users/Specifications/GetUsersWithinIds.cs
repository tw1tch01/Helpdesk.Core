using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Data.Specifications;
using Helpdesk.Domain.Users;

[assembly: InternalsVisibleTo("Helpdesk.Services.UnitTests")]

namespace Helpdesk.Services.Users.Specifications
{
    public class GetUsersWithinIds : LinqSpecification<User>
    {
        internal readonly IList<int> _userIds;

        public GetUsersWithinIds(IList<int> userIds)
        {
            _userIds = userIds;
        }

        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => _userIds.Contains(user.UserId);
        }
    }
}