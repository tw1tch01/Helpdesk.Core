using System.Collections.Generic;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Users.Results.Enums;

namespace Helpdesk.Services.Users.Results
{
    public class CreateUserResult : IProcessResult<UserCreateResult>, IValidationResult
    {
        public CreateUserResult(UserCreateResult result)
        {
            Result = result;
        }

        public UserCreateResult Result { get; }

        public string Message => GetMessage();

        public int? UserId { get; internal set; }

        public string Username { get; internal set; }

        public Dictionary<string, List<string>> ValidationFailures { get; internal set; }

        #region Methods

        private string GetMessage()
        {
            return Result switch
            {
                _ => Result.ToString()
            };
        }

        #endregion Methods
    }
}