using System.Threading.Tasks;
using Helpdesk.DomainModels.Users;
using Helpdesk.Services.Users.Results;

namespace Helpdesk.Services.Users.Commands.UpdateUser
{
    public interface IUpdateUserService
    {
        Task<UpdateUserResult> Update(int userId, UserUpdates userUpdates);
    }
}