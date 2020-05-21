using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Users.Results;

namespace Helpdesk.Services.Users.Factories.CreateUser
{
    public interface ICreateUserResultFactory
    {
        CreateUserResult Created(User user);

        CreateUserResult DuplicateUsername(string username);

        CreateUserResult ValidationFailure(IList<ValidationFailure> errors);
    }
}