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

        public Dictionary<string, List<string>> ValidationFailures { get; }

        public IReadOnlyDictionary<string, ValueChange> PropertyChanges { get; }

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