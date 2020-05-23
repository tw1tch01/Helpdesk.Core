using System;
using System.Collections.Generic;
using Helpdesk.DomainModels.Users.Enums;

namespace Helpdesk.DomainModels.Users
{
    public class UserLookupParams
    {
        public DateTimeOffset? CreatedAfter { get; set; }
        public DateTimeOffset? CreatedBefore { get; set; }
        public string SearchTerm { get; set; }
        public IList<int> UserIds { get; set; } = new List<int>();
        public IList<Guid> UserGuids { get; set; } = new List<Guid>();
        public SortUsersBy? SortBy { get; set; }
    }
}