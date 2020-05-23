using System;
using System.Threading.Tasks;
using Helpdesk.DomainModels.Users;

namespace Helpdesk.Services.Users.Queries.GetUser
{
    public interface IGetUserService
    {
        Task<UserDetails> GetUser(int userId);

        Task<UserDetails> GetUser(Guid userGuid);
    }
}