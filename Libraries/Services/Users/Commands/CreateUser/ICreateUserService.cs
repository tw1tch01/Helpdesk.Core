using System.Threading.Tasks;
using Helpdesk.DomainModels.Users;
using Helpdesk.Services.Users.Results;

namespace Helpdesk.Services.Users.Commands.CreateUser
{
    public interface ICreateUserService
    {
        Task<CreateUserResult> Create(NewUser newUser);
    }
}