using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Users.Results.Enums;

namespace Helpdesk.Services.Users.Results
{
    public class DeleteUserResult : IProcessResult<UserDeleteResult>
    {
        public DeleteUserResult(UserDeleteResult result)
        {
            Result = result;
        }

        public UserDeleteResult Result { get; }
        public string Message => GetMessage();
        public int UserId { get; set; }

        #region Methods

        private string GetMessage()
        {
            return Result switch
            {
                UserDeleteResult.Deleted => ResultMessages.Deleted,
                UserDeleteResult.UserNotFound => ResultMessages.UserNotFound,
                _ => Result.ToString()
            };
        }

        #endregion Methods
    }
}