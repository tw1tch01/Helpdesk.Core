using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using FluentValidation;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Users;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Users.Factories.CreateUser;
using Helpdesk.Services.Users.Results;
using Helpdesk.Services.Users.Specifications;

namespace Helpdesk.Services.Users.Commands.CreateUser
{
    public class CreateUserService : ICreateUserService
    {
        private readonly IContextRepository<IUserContext> _repository;
        private readonly IMapper _mapper;
        private readonly ICreateUserResultFactory _factory;
        private readonly IValidator<NewUser> _validator;

        public CreateUserService(
            IContextRepository<IUserContext> repository,
            IMapper mapper,
            ICreateUserResultFactory factory,
            IValidator<NewUser> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _factory = factory;
            _validator = validator;
        }

        public virtual async Task<CreateUserResult> Create(NewUser newUser)
        {
            if (newUser == null) throw new ArgumentNullException(nameof(newUser));

            var validationResult = await _validator.ValidateAsync(newUser);

            if (!validationResult.IsValid) return _factory.ValidationFailure(validationResult.Errors);

            var existingUser = await _repository.SingleAsync(new GetUserByUsername(newUser.Username));

            if (existingUser != null) return _factory.DuplicateUsername(existingUser.Username);

            var user = _mapper.Map<User>(newUser);

            await _repository.AddAsync(user);
            await _repository.SaveAsync();

            return _factory.Created(user);
        }
    }
}