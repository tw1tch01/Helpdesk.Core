using System.Collections.Generic;
using Helpdesk.DomainModels.Common;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Users.Results.Enums;

namespace Helpdesk.Services.Users.Results
{
    public class UpdateUserResult : IProcessResult<UserUpdateResult>, IValidationResult, IUpdateResult
    {
        public UpdateUserResult(UserUpdateResult result)
        {
            Result = result;
        }

        public UserUpdateResult Result { get; }
        public string Message => GetMessage();
        public int UserId { get; internal set; }
        public string Username { get; internal set; }
        public Dictionary<string, List<string>> ValidationFailures { get; internal set; }
        public IReadOnlyDictionary<string, ValueChange> PropertyChanges { get; internal set; }

        #region Methods

        private string GetMessage()
        {
            return Result switch
            {
                UserUpdateResult.Updated => ResultMessages.Updated,
                UserUpdateResult.UserNotFound => ResultMessages.UserNotFound,
                UserUpdateResult.ValidationFailure => ResultMessages.ValidationFailure,
                UserUpdateResult.DuplicateUsername => ResultMessages.DuplicateUsername,
                _ => Result.ToString()
            };
        }

        #endregion Methods
    }
}