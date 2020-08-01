using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Data.Repositories;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Users;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Users.Queries.GetUser;
using Helpdesk.Services.Users.Specifications;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Queries
{
    [TestFixture]
    public class GetUserServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task GetUser_ById_VerifySingleAsyncForGetUserByIdIsCalled()
        {
            var userId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();

            var service = CreateService(
                mockRepository);

            await service.GetUser(userId);

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetUserById>(i => i.UserId == userId)), Times.Once, "Should call the repository's SingleAssync method of GetUserById exactly once.");
        }

        [Test]
        public async Task GetUser_ById_WhenUserRecordIsNull_ReturnsNull()
        {
            var mockRepository = new Mock<IContextRepository<IUserContext>>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync((User)null);

            var service = CreateService(
                mockRepository);

            var result = await service.GetUser(It.IsAny<int>());

            Assert.IsNull(result, "Should return null.");
        }

        [Test]
        public async Task GetUser_ById_VerifyMapperMapsUserRecordToUserDetails()
        {
            var user = _fixture.Create<User>();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockMapper = new Mock<IMapper>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(user);

            var service = CreateService(
                mockRepository,
                mockMapper);

            await service.GetUser(It.IsAny<int>());

            mockMapper.Verify(v => v.Map<UserDetails>(user), Times.Once, "Should map the user record to user details.");
        }

        [Test]
        public async Task GetUser_ById_WhenUserRecordExists_ReturnsMappedUserDetails()
        {
            var user = _fixture.Create<User>();
            var userDetails = _fixture.Create<UserDetails>();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockMapper = new Mock<IMapper>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(user);
            mockMapper.Setup(s => s.Map<UserDetails>(user)).Returns(userDetails);

            var service = CreateService(
                mockRepository,
                mockMapper);

            var result = await service.GetUser(It.IsAny<int>());

            Assert.AreEqual(userDetails, result, "Should return mapped user details.");
        }

        [Test]
        public async Task GetUser_ByIdentifier_VerifySingleAsyncForGetUserByIdentifierIsCalled()
        {
            var userGuid = _fixture.Create<Guid>();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();

            var service = CreateService(
                mockRepository);

            await service.GetUser(userGuid);

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetUserByIdentifier>(i => i.Identifier == userGuid)), Times.Once, "Should call the repository's SingleAssync method of GetUserByIdentifier exactly once.");
        }

        [Test]
        public async Task GetUser_ByIdentifier_WhenUserRecordIsNull_ReturnsNull()
        {
            var mockRepository = new Mock<IContextRepository<IUserContext>>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserByIdentifier>())).ReturnsAsync((User)null);

            var service = CreateService(
                mockRepository);

            var result = await service.GetUser(It.IsAny<Guid>());

            Assert.IsNull(result, "Should return null.");
        }

        [Test]
        public async Task GetUser_ByIdentifier_VerifyMapperMapsUserRecordToUserDetails()
        {
            var user = _fixture.Create<User>();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockMapper = new Mock<IMapper>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserByIdentifier>())).ReturnsAsync(user);

            var service = CreateService(
                mockRepository,
                mockMapper);

            await service.GetUser(It.IsAny<Guid>());

            mockMapper.Verify(v => v.Map<UserDetails>(user), Times.Once, "Should map the user record to user details.");
        }

        [Test]
        public async Task GetUser_ByIdentifier_WhenUserRecordExists_ReturnsMappedUserDetails()
        {
            var user = _fixture.Create<User>();
            var userDetails = _fixture.Create<UserDetails>();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockMapper = new Mock<IMapper>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserByIdentifier>())).ReturnsAsync(user);
            mockMapper.Setup(s => s.Map<UserDetails>(user)).Returns(userDetails);

            var service = CreateService(
                mockRepository,
                mockMapper);

            var result = await service.GetUser(It.IsAny<Guid>());

            Assert.AreEqual(userDetails, result, "Should return mapped user details.");
        }

        private GetUserService CreateService(
            Mock<IContextRepository<IUserContext>> mockRepository = null,
            Mock<IMapper> mockMapper = null)
        {
            mockRepository ??= new Mock<IContextRepository<IUserContext>>();
            mockMapper ??= new Mock<IMapper>();

            return new GetUserService(
                mockRepository.Object,
                mockMapper.Object);
        }
    }
}