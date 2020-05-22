using System.Threading.Tasks;
using Helpdesk.Services.Users.Results;

namespace Helpdesk.Services.Users.Commands.DeleteUser
{
    public interface IDeleteUserService
    {
        Task<DeleteUserResult> Delete(int userId);

        Task<DeleteUserResult> Delete(string username);
    }
}