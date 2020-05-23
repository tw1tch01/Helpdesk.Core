using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Common;
using Helpdesk.DomainModels.Users;

namespace Helpdesk.Services.Users.Queries.LookupUsers
{
    public interface ILookupUsersService
    {
        Task<IList<UserLookup>> Lookup(UserLookupParams @params);

        Task<PagedCollection<UserLookup>> PagedLookup(int page, int pageSize, UserLookupParams @params);
    }
}