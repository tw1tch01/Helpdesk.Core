using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Users;

namespace Helpdesk.Services.Users.Specifications
{
    public class GetUsersWithinIds : LinqSpecification<User>
    {
        public GetUsersWithinIds(IList<int> userIds)
        {
            UserIds = userIds;
        }

        public IList<int> UserIds { get; }

        public override Expression<Func<User, bool>> AsExpression()
        {
            return user => UserIds.Contains(user.UserId);
        }
    }
}