using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Users;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Users.Commands.DeleteUser;
using Helpdesk.Services.Users.Factories.DeleteUser;
using Helpdesk.Services.Users.Specifications;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Commands
{
    [TestFixture]
    public class DeleteUserServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task Delete_VerifySingleAsyncForGetUserByIdIsCalled()
        {
            var userId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();

            var service = CreateService(
                mockRepository: mockRepository);

            await service.Delete(userId);

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetUserById>(a => a._userId == userId)), Times.Once, "Should call the SingleAsync method exactly once for GetUserById.");
        }

        [Test]
        public async Task Delete_WhenUserRecordIsNull_VerifyFactoryUserNotFoundIsReturned()
        {
            var userId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockFactory = new Mock<IDeleteUserResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync((User)null);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Delete(userId);

            mockFactory.Verify(v => v.UserNotFound(userId), Times.Once, "Should return the factory's UserNotFound method.");
        }

        [Test]
        public async Task Delete_VerifyRemoveIsCalled()
        {
            var userId = _fixture.Create<int>();
            var user = new User();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(user);

            var service = CreateService(
                mockRepository: mockRepository);

            await service.Delete(userId);

            mockRepository.Verify(v => v.Remove(user), Times.Once, "Should call Remove for the user.");
        }

        [Test]
        public async Task Delete_VerifySaveAsyncIsCalled()
        {
            var userId = _fixture.Create<int>();
            var user = new User();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(user);

            var service = CreateService(
                mockRepository: mockRepository);

            await service.Delete(userId);

            mockRepository.Verify(v => v.SaveAsync(), Times.Once, "Should call SaveAsync.");
        }

        [Test]
        public async Task Delete_WhenUserIsDeleted_VerifyFactoryDeletedIsReturned()
        {
            var userId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockFactory = new Mock<IDeleteUserResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Delete(userId);

            mockFactory.Verify(v => v.Deleted(userId), Times.Once, "Should return the factory's Deleted method.");
        }

        private DeleteUserService CreateService(
            IMock<IContextRepository<IUserContext>> mockRepository = null,
            IMock<IDeleteUserResultFactory> mockFactory = null)
        {
            mockRepository ??= new Mock<IContextRepository<IUserContext>>();
            mockFactory ??= new Mock<IDeleteUserResultFactory>();

            return new DeleteUserService(
                mockRepository.Object,
                mockFactory.Object);
        }
    }
}