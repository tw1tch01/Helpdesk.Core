using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Users;
using Helpdesk.Services.Extensions;
using Helpdesk.Services.Users.Results;
using Helpdesk.Services.Users.Results.Enums;

namespace Helpdesk.Services.Users.Factories.CreateUser
{
    public class CreateUserResultFactory : ICreateUserResultFactory
    {
        public CreateUserResult Created(User user)
        {
            return new CreateUserResult(UserCreateResult.Created)
            {
                UserId = user.UserId,
                Username = user.Username
            };
        }

        public CreateUserResult DuplicateUsername(string username)
        {
            return new CreateUserResult(UserCreateResult.DuplicateUsername)
            {
                Username = username
            };
        }

        public CreateUserResult ValidationFailure(IList<ValidationFailure> errors)
        {
            return new CreateUserResult(UserCreateResult.ValidationFailure)
            {
                ValidationFailures = errors.GroupPropertyWithErrors()
            };
        }
    }
}