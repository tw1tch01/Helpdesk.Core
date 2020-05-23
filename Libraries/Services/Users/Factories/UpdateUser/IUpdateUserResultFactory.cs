using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Common;
using Helpdesk.Services.Users.Results;

namespace Helpdesk.Services.Users.Factories.UpdateUser
{
    public interface IUpdateUserResultFactory
    {
        UpdateUserResult DuplicateUsername(User existingUser);

        UpdateUserResult Updated(User user, IReadOnlyDictionary<string, ValueChange> readOnlyDictionary);

        UpdateUserResult UserNotFound(int userId);

        UpdateUserResult ValidationFailure(int userId, IList<ValidationFailure> errors);
    }
}