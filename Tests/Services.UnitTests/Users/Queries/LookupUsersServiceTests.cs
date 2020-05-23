using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Data.Common;
using Data.Repositories;
using Data.Specifications;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Users;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Users.Queries.LookupUsers;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Queries
{
    [TestFixture]
    public class LookupUsersServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        #region Lookup

        [Test]
        public async Task Lookup_VerifyListAsyncIsCalled()
        {
            var @params = _fixture.Create<UserLookupParams>();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();

            var service = CreateService(
                mockRepository);

            await service.Lookup(@params);

            mockRepository.Verify(v => v.ListAsync(It.IsAny<LinqSpecification<User>>()), Times.Once, "Should call ListAsync method exactly once.");
        }

        [Test]
        public async Task Lookup_VerifyMapperMapsUsersToListOfUserLookup()
        {
            var @params = _fixture.Create<UserLookupParams>();
            var users = _fixture.CreateMany<User>().ToList();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockMapper = new Mock<IMapper>();

            mockRepository.Setup(s => s.ListAsync(It.IsAny<LinqSpecification<User>>())).ReturnsAsync(users);

            var service = CreateService(
                mockRepository,
                mockMapper);

            await service.Lookup(@params);

            mockMapper.Verify(v => v.Map<IList<UserLookup>>(users), Times.Once, "Should map user records to list of UserLookup.");
        }

        [Test]
        public async Task Lookup_ReturnsMappedListOfUserLookup()
        {
            var @params = _fixture.Create<UserLookupParams>();
            var users = _fixture.CreateMany<User>().ToList();
            var userLookup = _fixture.CreateMany<UserLookup>().ToList();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockMapper = new Mock<IMapper>();

            mockRepository.Setup(s => s.ListAsync(It.IsAny<LinqSpecification<User>>())).ReturnsAsync(users);
            mockMapper.Setup(s => s.Map<IList<UserLookup>>(users)).Returns(userLookup);

            var service = CreateService(
                mockRepository,
                mockMapper);

            var result = await service.Lookup(@params);

            Assert.AreEqual(userLookup, result, "Should return mapped list of UserLookup.");
        }

        #endregion Lookup

        #region PagedLookup

        [Test]
        public async Task PagedLookup_VerifyPagedListAsyncIsCalled()
        {
            var @params = _fixture.Create<UserLookupParams>();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();

            var service = CreateService(
                mockRepository);

            mockRepository.Setup(s => s.PagedListAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<LinqSpecification<User>>(), u => u.UserId)).ReturnsAsync(_fixture.Create<PagedCollection<User>>());
            
            await service.PagedLookup(It.IsAny<int>(), It.IsAny<int>(), @params);

            mockRepository.Verify(v => v.PagedListAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<LinqSpecification<User>>(), u => u.UserId), Times.Once, "Should call PagedListAsync method exactly once.");
        }

        [Test]
        public async Task PagedLookup_VerifyMapperMapsUsersToListOfUserPagedLookup()
        {
            var @params = _fixture.Create<UserLookupParams>();
            var pagedCollection = _fixture.Create<PagedCollection<User>>();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockMapper = new Mock<IMapper>();

            mockRepository.Setup(s => s.PagedListAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<LinqSpecification<User>>(), u => u.UserId)).ReturnsAsync(pagedCollection);

            var service = CreateService(
                mockRepository,
                mockMapper);

            await service.PagedLookup(It.IsAny<int>(), It.IsAny<int>(), @params);

            mockMapper.Verify(v => v.Map<IList<UserLookup>>(pagedCollection.Items), Times.Once, "Should map user records to paged collection of UserLookup.");
        }

        [Test]
        public async Task PagedLookup_ReturnsMappedListOfUserPagedLookup()
        {
            var @params = _fixture.Create<UserLookupParams>();
            var pagedCollection = _fixture.Create<PagedCollection<User>>();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var details = _fixture.CreateMany<UserLookup>().ToList();
            var mockMapper = new Mock<IMapper>();

            var expected = new PagedCollection<UserLookup>
            (
                pagedCollection.Page,
                pagedCollection.PageSize,
                pagedCollection.TotalRecords,
                details
            );

            mockRepository.Setup(s => s.PagedListAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<LinqSpecification<User>>(), u => u.UserId)).ReturnsAsync(pagedCollection);
            mockMapper.Setup(s => s.Map<IList<UserLookup>>(pagedCollection.Items)).Returns(details);

            var service = CreateService(
                mockRepository,
                mockMapper);

            var result = await service.PagedLookup(It.IsAny<int>(), It.IsAny<int>(), @params);

            Assert.AreEqual(expected.Items, result.Items, "Should return PagedCollection of mapped UserLookup.");
        }

        #endregion PagedLookup

        private LookupUsersService CreateService(
            Mock<IContextRepository<IUserContext>> mockRepository = null,
            Mock<IMapper> mockMapper = null)
        {
            mockRepository ??= new Mock<IContextRepository<IUserContext>>();
            mockMapper ??= new Mock<IMapper>();

            return new LookupUsersService(
                mockRepository.Object,
                mockMapper.Object);
        }
    }
}