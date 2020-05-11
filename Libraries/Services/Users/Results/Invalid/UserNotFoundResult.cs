namespace Helpdesk.Services.Users.Results.Invalid
{
    public class UserNotFoundResult : UserInvalidResult
    {
        private const string _message = "User record was not found.";

        public UserNotFoundResult(int userId)
            : base(userId, _message)
        {
        }
    }
}