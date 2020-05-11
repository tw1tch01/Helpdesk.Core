namespace Helpdesk.Services.Users.Results.Invalid
{
    public abstract class UserInvalidResult : UserValidationResult
    {
        protected UserInvalidResult(string message)
            : base(false, message)
        {
        }

        protected UserInvalidResult(int userId, string message)
            : base(userId, false, message)
        {
        }
    }
}