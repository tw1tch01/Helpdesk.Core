using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Data.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Helpdesk.Domain.Common;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Users;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Users.Commands.UpdateUser;
using Helpdesk.Services.Users.Factories.UpdateUser;
using Helpdesk.Services.Users.Specifications;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Commands
{
    [TestFixture]
    public class UpdateUserServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void Update_WhenEditUserIsNull_ThrowsArgumentNullException()
        {
            var service = CreateService();
            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.Update(It.IsAny<int>(), null));
        }

        [Test]
        public async Task Update_WhenEditUserIsNotValid_VerifyFactoryValidationFailureIsResult()
        {
            var userId = _fixture.Create<int>();
            var editUser = new EditUser();
            var mockFactory = new Mock<IUpdateUserResultFactory>();
            var mockValidator = new Mock<IValidator<EditUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(false);
            mockValidator.Setup(s => s.ValidateAsync(editUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);

            var service = CreateService(
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            await service.Update(userId, editUser);

            mockFactory.Verify(v => v.ValidationFailure(userId, mockValidationResult.Object.Errors), Times.Once, "Should return the factory's ValidationFailure method.");
        }

        [Test]
        public async Task Update_VerifySingleAsyncForGetUserByIdIsCalled()
        {
            var userId = _fixture.Create<int>();
            var editUser = new EditUser();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockValidator = new Mock<IValidator<EditUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(true);
            mockValidator.Setup(s => s.ValidateAsync(editUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockValidator: mockValidator);

            await service.Update(userId, editUser);

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetUserById>(a => a.UserId == userId)), Times.Once, "Should call the repository's SingleAsync method for GetUserById exactly once.");
        }

        [Test]
        public async Task Update_WhenUserRecordIsNull_VerifyFactoryUserNotFoundIsReturned()
        {
            var userId = _fixture.Create<int>();
            var editUser = new EditUser();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockFactory = new Mock<IUpdateUserResultFactory>();
            var mockValidator = new Mock<IValidator<EditUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(true);
            mockValidator.Setup(s => s.ValidateAsync(editUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync((User)null);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            await service.Update(userId, editUser);

            mockFactory.Verify(v => v.UserNotFound(userId), Times.Once, "Should return the factory's UserNotFound method.");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task Update_WhenUsernameIsNullOrEmpty_VerifySingleAsyncForGetUserByUsernameIsNotCalled(string username)
        {
            var userId = _fixture.Create<int>();
            var editUser = new EditUser
            {
                Username = username
            };
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockValidator = new Mock<IValidator<EditUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(true);
            mockValidator.Setup(s => s.ValidateAsync(editUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());

            var service = CreateService(
                mockRepository: mockRepository,
                mockValidator: mockValidator);

            await service.Update(userId, editUser);

            mockRepository.Verify(v => v.SingleAsync(It.IsAny<GetUserByUsername>()), Times.Never, "Should not call the SingleAsync method exactly once for GetUserByUsername.");
        }

        [Test]
        public async Task Update_WhenUsernameIsNotEmpty_VerifySingleAsyncForGetUserByUsername()
        {
            var userId = _fixture.Create<int>();
            var editUser = new EditUser
            {
                Username = _fixture.Create<string>()
            };
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockValidator = new Mock<IValidator<EditUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(true);
            mockValidator.Setup(s => s.ValidateAsync(editUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());

            var service = CreateService(
                mockRepository: mockRepository,
                mockValidator: mockValidator);

            await service.Update(userId, editUser);

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetUserByUsername>(a => a.Username == editUser.Username)), Times.Once, "Should call the SingleAsync method exactly once for GetUserByUsername.");
        }

        [Test]
        public async Task Update_WhenUsernameAlreadyInUse_VerifyFactoryDuplicateUsernameIsReturned()
        {
            var userId = _fixture.Create<int>();
            var editUser = new EditUser
            {
                Username = _fixture.Create<string>()
            };
            var user = new User();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockFactory = new Mock<IUpdateUserResultFactory>();
            var mockValidator = new Mock<IValidator<EditUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(true);
            mockValidator.Setup(s => s.ValidateAsync(editUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserByUsername>())).ReturnsAsync(user);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            await service.Update(userId, editUser);

            mockFactory.Verify(v => v.DuplicateUsername(user), Times.Once, "Should return the factory's DuplicateUsername method.");
        }

        [Test]
        public async Task Update_VerifyMapperMapsChangesToUserRecord()
        {
            var userId = _fixture.Create<int>();
            var editUser = new EditUser();
            var user = new User();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<EditUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(true);
            mockValidator.Setup(s => s.ValidateAsync(editUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(user);

            var service = CreateService(
                mockRepository: mockRepository,
                mockMapper: mockMapper,
                mockValidator: mockValidator);

            await service.Update(userId, editUser);

            mockMapper.Verify(v => v.Map(editUser, user), Times.Once, "Should map the editUser changes to the user record.");
        }

        [Test]
        public async Task Update_VerifySaveAsyncIsCalled()
        {
            var userId = _fixture.Create<int>();
            var editUser = new EditUser();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockValidator = new Mock<IValidator<EditUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(true);
            mockValidator.Setup(s => s.ValidateAsync(editUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());

            var service = CreateService(
                mockRepository: mockRepository,
                mockValidator: mockValidator);

            await service.Update(userId, editUser);

            mockRepository.Verify(v => v.SaveAsync(), Times.Once, "Should call the SaveAsync for the user.");
        }

        [Test]
        public async Task Update_WhenUserIsUpdated_VerifyFactoryUpdatedIsReturned()
        {
            var userId = _fixture.Create<int>();
            var editUser = new EditUser();
            var user = new User();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockMapper = new Mock<IMapper>();
            var mockFactory = new Mock<IUpdateUserResultFactory>();
            var mockValidator = new Mock<IValidator<EditUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(true);
            mockValidator.Setup(s => s.ValidateAsync(editUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(user);
            mockMapper.Setup(s => s.Map<User>(editUser)).Returns(user);

            var service = CreateService(
                mockRepository: mockRepository,
                mockMapper: mockMapper,
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            await service.Update(userId, editUser);

            mockFactory.Verify(v => v.Updated(user, It.IsAny<IReadOnlyDictionary<string, ValueChange>>()), Times.Once, "Should return the factory's Updated method.");
        }

        private UpdateUserService CreateService(
            IMock<IContextRepository<IUserContext>> mockRepository = null,
            IMock<IMapper> mockMapper = null,
            IMock<IUpdateUserResultFactory> mockFactory = null,
            IMock<IValidator<EditUser>> mockValidator = null)
        {
            mockRepository ??= new Mock<IContextRepository<IUserContext>>();
            mockMapper ??= new Mock<IMapper>();
            mockFactory ??= new Mock<IUpdateUserResultFactory>();
            mockValidator ??= new Mock<IValidator<EditUser>>();

            return new UpdateUserService(
                mockRepository.Object,
                mockMapper.Object,
                mockFactory.Object,
                mockValidator.Object);
        }
    }
}