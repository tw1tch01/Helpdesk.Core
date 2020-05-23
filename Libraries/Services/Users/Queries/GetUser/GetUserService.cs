using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using Helpdesk.DomainModels.Users;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Users.Specifications;

namespace Helpdesk.Services.Users.Queries.GetUser
{
    public class GetUserService : IGetUserService
    {
        private readonly IContextRepository<IUserContext> _repository;
        private readonly IMapper _mapper;

        public GetUserService(
            IContextRepository<IUserContext> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<UserDetails> GetUser(int userId)
        {
            var user = await _repository.SingleAsync(new GetUserById(userId).AsNoTracking());

            if (user == null) return null;

            var details = _mapper.Map<UserDetails>(user);

            return details;
        }

        public virtual async Task<UserDetails> GetUser(Guid userGuid)
        {
            var user = await _repository.SingleAsync(new GetUserByIdentifier(userGuid).AsNoTracking());

            if (user == null) return null;

            var details = _mapper.Map<UserDetails>(user);

            return details;
        }
    }
}