using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data.Common;
using Data.Repositories;
using Data.Specifications;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Users;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Users.Specifications;

namespace Helpdesk.Services.Users.Queries.LookupUsers
{
    public class LookupUsersService : AbstractUserLookup, ILookupUsersService
    {
        private readonly IContextRepository<IUserContext> _repository;
        private readonly IMapper _mapper;

        public LookupUsersService(
            IContextRepository<IUserContext> repository,
            IMapper mapper)
            : base(GetDefaultSpecification())
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<IList<UserLookup>> Lookup(UserLookupParams @params)
        {
            WithParameters(@params);

            var users = await _repository.ListAsync(_specification);

            var details = _mapper.Map<IList<UserLookup>>(users);

            return details;
        }

        public virtual async Task<PagedCollection<UserLookup>> PagedLookup(int page, int pageSize, UserLookupParams @params)
        {
            WithParameters(@params);

            (page, pageSize) = ValidatePaging(page, pageSize);

            var pagedCollection = await _repository.PagedListAsync(page, pageSize, _specification, u => u.UserId);

            var details = _mapper.Map<IList<UserLookup>>(pagedCollection.Items);

            return new PagedCollection<UserLookup>
            (
                pagedCollection.Page,
                pagedCollection.PageSize,
                pagedCollection.TotalRecords,
                details
            );
        }

        private static LinqSpecification<User> GetDefaultSpecification()
        {
            return new GetAllUsers().AsNoTracking();
        }
    }
}