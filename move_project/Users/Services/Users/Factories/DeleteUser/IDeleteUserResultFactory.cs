using Helpdesk.Services.Users.Results;

namespace Helpdesk.Services.Users.Factories.DeleteUser
{
    public interface IDeleteUserResultFactory
    {
        DeleteUserResult Deleted(int userId);

        DeleteUserResult Deleted(string username);

        DeleteUserResult UserNotFound(int userId);

        DeleteUserResult UserNotFound(string username);
    }
}