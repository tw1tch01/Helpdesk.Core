using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using FluentValidation;
using Helpdesk.DomainModels.Users;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Users.Factories.UpdateUser;
using Helpdesk.Services.Users.Results;
using Helpdesk.Services.Users.Specifications;

namespace Helpdesk.Services.Users.Commands.UpdateUser
{
    public class UpdateUserService : IUpdateUserService
    {
        private readonly IContextRepository<IUserContext> _repository;
        private readonly IMapper _mapper;
        private readonly IUpdateUserResultFactory _factory;
        private readonly IValidator<EditUser> _validator;

        public UpdateUserService(
            IContextRepository<IUserContext> repository,
            IMapper mapper,
            IUpdateUserResultFactory factory,
            IValidator<EditUser> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _factory = factory;
            _validator = validator;
        }

        public virtual async Task<UpdateUserResult> Update(int userId, EditUser editUser)
        {
            if (editUser == null) throw new ArgumentNullException(nameof(editUser));

            var validationResult = await _validator.ValidateAsync(editUser);

            if (!validationResult.IsValid) return _factory.ValidationFailure(userId, validationResult.Errors);

            var user = await _repository.SingleAsync(new GetUserById(userId));

            if (user == null) return _factory.UserNotFound(userId);

            if (!string.IsNullOrWhiteSpace(editUser.Username))
            {
                var existingUser = await _repository.SingleAsync(new GetUserByUsername(editUser.Username));

                if (existingUser != null) return _factory.DuplicateUsername(existingUser);
            }

            _mapper.Map(editUser, user);
            await _repository.SaveAsync();

            return _factory.Updated(user, editUser.GetChanges(user));
        }
    }
}