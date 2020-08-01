using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Common;
using Helpdesk.Domain.Users;
using Helpdesk.Services.Extensions;
using Helpdesk.Services.Users.Results;
using Helpdesk.Services.Users.Results.Enums;

namespace Helpdesk.Services.Users.Factories.UpdateUser
{
    public class UpdateUserResultFactory : IUpdateUserResultFactory
    {
        public UpdateUserResult DuplicateUsername(User existingUser)
        {
            return new UpdateUserResult(UserUpdateResult.DuplicateUsername)
            {
                UserId = existingUser.UserId,
                Username = existingUser.Username
            };
        }

        public UpdateUserResult Updated(User user, IReadOnlyDictionary<string, ValueChange> changes)
        {
            return new UpdateUserResult(UserUpdateResult.Updated)
            {
                UserId = user.UserId,
                PropertyChanges = changes
            };
        }

        public UpdateUserResult UserNotFound(int userId)
        {
            return new UpdateUserResult(UserUpdateResult.UserNotFound)
            {
                UserId = userId
            };
        }

        public UpdateUserResult ValidationFailure(int userId, IList<ValidationFailure> errors)
        {
            return new UpdateUserResult(UserUpdateResult.ValidationFailure)
            {
                UserId = userId,
                ValidationFailures = errors.GroupPropertyWithErrors()
            };
        }
    }
}