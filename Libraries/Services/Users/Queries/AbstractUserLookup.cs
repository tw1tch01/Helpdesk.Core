using System;
using System.Collections.Generic;
using System.Linq;
using Data.Specifications;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Users;
using Helpdesk.DomainModels.Users.Enums;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Specifications;
using Helpdesk.Services.Users.Specifications;

namespace Helpdesk.Services.Users.Queries
{
    public abstract class AbstractUserLookup : AbstractLookup<User>
    {
        protected AbstractUserLookup(LinqSpecification<User> specification)
            : base(specification)
        {
        }

        #region Methods

        protected AbstractUserLookup WithParameters(UserLookupParams @params)
        {
            if (@params == null) throw new ArgumentNullException(nameof(@params));

            if (@params.CreatedAfter.HasValue) CreatedAfter(@params.CreatedAfter.Value);

            if (@params.CreatedBefore.HasValue) CreatedBefore(@params.CreatedBefore.Value);

            if (!string.IsNullOrWhiteSpace(@params.SearchTerm)) SearchBy(@params.SearchTerm);

            if (@params.UserIds.Any()) WithinUserId(@params.UserIds);

            if (@params.UserGuids.Any()) WithinUserGuids(@params.UserGuids);

            if (@params.SortBy.HasValue) SortBy(@params.SortBy.Value);

            return this;
        }

        #endregion Methods

        #region Private Methods

        private void CreatedAfter(DateTimeOffset createdAfter)
        {
            And(new CreatedAfter<User>(createdAfter));
        }

        private void CreatedBefore(DateTimeOffset createdBefore)
        {
            And(new CreatedBefore<User>(createdBefore));
        }

        private void SearchBy(string term)
        {
            var searchSpec = new UserNameContainsTerm(term)
                           | new UserSurnameContainsTerm(term)
                           | new UserUsernameContainsTerm(term);

            And(searchSpec);
        }

        private void WithinUserId(IList<int> userIds)
        {
            And(new GetUsersWithinIds(userIds));
        }

        private void WithinUserGuids(IList<Guid> userGuids)
        {
            And(new GetUsersWithinIdentifiers(userGuids));
        }

        private void SortBy(SortUsersBy sortBy)
        {
            switch (sortBy)
            {
                case SortUsersBy.NameAsc:
                    _specification.OrderBy(o => o.Name);
                    break;

                case SortUsersBy.NameDesc:
                    _specification.OrderByDescending(o => o.Name);
                    break;

                case SortUsersBy.SurnameAsc:
                    _specification.OrderBy(o => o.Surname);

                    break;

                case SortUsersBy.SurnameDesc:
                    _specification.OrderByDescending(o => o.Surname);
                    break;

                case SortUsersBy.UsernameAsc:
                    _specification.OrderBy(o => o.Username);
                    break;

                case SortUsersBy.UsernameDesc:
                    _specification.OrderByDescending(o => o.Username);
                    break;
            }
        }

        #endregion Private Methods
    }
}