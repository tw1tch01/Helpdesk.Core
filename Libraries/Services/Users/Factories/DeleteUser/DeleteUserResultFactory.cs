using Helpdesk.Services.Users.Results;
using Helpdesk.Services.Users.Results.Enums;

namespace Helpdesk.Services.Users.Factories.DeleteUser
{
    public class DeleteUserResultFactory : IDeleteUserResultFactory
    {
        public DeleteUserResult Deleted(int userId)
        {
            return new DeleteUserResult(UserDeleteResult.Deleted)
            {
                UserId = userId
            };
        }

        public DeleteUserResult UserNotFound(int userId)
        {
            return new DeleteUserResult(UserDeleteResult.UserNotFound)
            {
                UserId = userId
            };
        }
    }
}