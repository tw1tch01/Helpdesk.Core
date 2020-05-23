using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Users.Factories.DeleteUser;
using Helpdesk.Services.Users.Results;
using Helpdesk.Services.Users.Specifications;

namespace Helpdesk.Services.Users.Commands.DeleteUser
{
    public class DeleteUserService : IDeleteUserService
    {
        private readonly IContextRepository<IUserContext> _repository;
        private readonly IDeleteUserResultFactory _factory;

        public DeleteUserService(
            IContextRepository<IUserContext> repository,
            IDeleteUserResultFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public virtual async Task<DeleteUserResult> Delete(int userId)
        {
            var user = await _repository.SingleAsync(new GetUserById(userId));

            if (user == null) return _factory.UserNotFound(userId);

            _repository.Remove(user);
            await _repository.SaveAsync();

            return _factory.Deleted(userId);
        }
    }
}