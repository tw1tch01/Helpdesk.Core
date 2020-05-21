using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using FluentValidation;
using Helpdesk.DomainModels.Users;
using Helpdesk.Services.Common;
using Helpdesk.Services.Users.Factories.UpdateUser;
using Helpdesk.Services.Users.Results;
using Helpdesk.Services.Users.Specifications;

namespace Helpdesk.Services.Users.Commands.UpdateUser
{
    public class UpdateUserService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IMapper _mapper;
        private readonly IUpdateUserResultFactory _factory;
        private readonly IValidator<UserUpdates> _validator;

        public UpdateUserService(
            IContextRepository<ITicketContext> repository,
            IMapper mapper,
            IUpdateUserResultFactory factory,
            IValidator<UserUpdates> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _factory = factory;
            _validator = validator;
        }

        public virtual async Task<UpdateUserResult> Update(int userId, UserUpdates userUpdates)
        {
            if (userUpdates == null) throw new ArgumentNullException(nameof(userUpdates));

            var validationResult = await _validator.ValidateAsync(userUpdates);

            if (!validationResult.IsValid) return _factory.ValidationFailure(userId, validationResult.Errors);

            var user = await _repository.SingleAsync(new GetUserById(userId));

            if (user == null) return _factory.UserNotFound(userId);

            _mapper.Map(userUpdates, user);
            await _repository.SaveAsync();

            return _factory.Updated(user, userUpdates.GetChanges(user));
        }

        public virtual async Task<UpdateUserResult> Update(string username, UserUpdates userUpdates)
        {
            if (userUpdates == null) throw new ArgumentNullException(nameof(userUpdates));

            var validationResult = await _validator.ValidateAsync(userUpdates);

            if (!validationResult.IsValid) return _factory.ValidationFailure(username, validationResult.Errors);

            var user = await _repository.SingleAsync(new GetUserByUsername(username));

            if (user == null) return _factory.UserNotFound(username);

            _mapper.Map(userUpdates, user);
            await _repository.SaveAsync();

            return _factory.Updated(user, userUpdates.GetChanges(user));
        }
    }
}