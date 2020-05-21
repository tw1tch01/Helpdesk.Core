using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Common;
using Helpdesk.Services.Users.Results;

namespace Helpdesk.Services.Users.Factories.UpdateUser
{
    public interface IUpdateUserResultFactory
    {
        UpdateUserResult Updated(User user, IReadOnlyDictionary<string, ValueChange> readOnlyDictionary);

        UpdateUserResult UserNotFound(int userId);

        UpdateUserResult UserNotFound(string username);

        UpdateUserResult ValidationFailure(int userId, IList<ValidationFailure> errors);

        UpdateUserResult ValidationFailure(string usernam, IList<ValidationFailure> errors);
    }
}