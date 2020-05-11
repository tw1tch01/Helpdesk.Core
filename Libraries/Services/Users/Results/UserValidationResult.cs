using Helpdesk.Domain.Entities;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Users.Results
{
    public abstract class UserValidationResult : ProcessResult
    {
        private const string _userIdKey = nameof(User.UserId);

        protected UserValidationResult(bool isValid, string message)
            : base(isValid, message)
        {
        }

        protected UserValidationResult(int userId, bool isValid, string message)
            : base(isValid, message)
        {
            Data[_userIdKey] = userId;
        }
    }
}