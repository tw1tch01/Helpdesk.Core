using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Users;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Users.Commands.CreateUser;
using Helpdesk.Services.Users.Factories.CreateUser;
using Helpdesk.Services.Users.Specifications;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Commands
{
    [TestFixture]
    public class CreateUserServiceTests
    {
        [Test]
        public void Create_WhenNewUserIsNull_ThrowsArgumentNullException()
        {
            var service = CreateService();
            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.Create(null));
        }

        [Test]
        public async Task Create_WhenNewUserIsNotValid_VerifyFactoryValidationFailureIsResult()
        {
            var newUser = new NewUser();
            var mockFactory = new Mock<ICreateUserResultFactory>();
            var mockValidator = new Mock<IValidator<NewUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(false);
            mockValidator.Setup(s => s.ValidateAsync(newUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);

            var service = CreateService(
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            await service.Create(newUser);

            mockFactory.Verify(v => v.ValidationFailure(mockValidationResult.Object.Errors), Times.Once, "Should return the factory's ValidationFailure method.");
        }

        [Test]
        public async Task Create_VerifySingleAsyncForGetUserByUsername()
        {
            var newUser = new NewUser();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockValidator = new Mock<IValidator<NewUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(true);
            mockValidator.Setup(s => s.ValidateAsync(newUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockValidator: mockValidator);

            await service.Create(newUser);

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetUserByUsername>(a => a.Username == newUser.Username)), Times.Once, "Should call the SingleAsync method exactly once for GetUserByUsername.");
        }

        [Test]
        public async Task Create_WhenUsernameAlreadyExists_VerifyFactoryDuplicateUsernameIsReturned()
        {
            var user = new User();
            var newUser = new NewUser();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockFactory = new Mock<ICreateUserResultFactory>();
            var mockValidator = new Mock<IValidator<NewUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(true);
            mockValidator.Setup(s => s.ValidateAsync(newUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserByUsername>())).ReturnsAsync(user);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            await service.Create(newUser);

            mockFactory.Verify(v => v.DuplicateUsername(user.Username), Times.Once, "Should return the factory's DuplicateUsername method.");
        }

        [Test]
        public async Task Create_VerifyAddAsyncIsCalled()
        {
            var newUser = new NewUser();
            var user = new User();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<NewUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(true);
            mockValidator.Setup(s => s.ValidateAsync(newUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserByUsername>())).ReturnsAsync((User)null);
            mockMapper.Setup(s => s.Map<User>(newUser)).Returns(user);

            var service = CreateService(
                mockRepository: mockRepository,
                mockMapper: mockMapper,
                mockValidator: mockValidator);

            await service.Create(newUser);

            mockRepository.Verify(v => v.AddAsync(user), Times.Once, "Should call the AddAsync for the user.");
        }

        [Test]
        public async Task Create_VerifySaveAsyncIsCalled()
        {
            var newUser = new NewUser();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockValidator = new Mock<IValidator<NewUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(true);
            mockValidator.Setup(s => s.ValidateAsync(newUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserByUsername>())).ReturnsAsync((User)null);

            var service = CreateService(
                mockRepository: mockRepository,
                mockValidator: mockValidator);

            await service.Create(newUser);

            mockRepository.Verify(v => v.SaveAsync(), Times.Once, "Should call the SaveAsync for the user.");
        }

        [Test]
        public async Task Create_WhenUserIsAdded_VerifyFactoryCreatedIsReturned()
        {
            var newUser = new NewUser();
            var user = new User();
            var mockRepository = new Mock<IContextRepository<IUserContext>>();
            var mockMapper = new Mock<IMapper>();
            var mockFactory = new Mock<ICreateUserResultFactory>();
            var mockValidator = new Mock<IValidator<NewUser>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidationResult.Setup(s => s.IsValid).Returns(true);
            mockValidator.Setup(s => s.ValidateAsync(newUser, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetUserByUsername>())).ReturnsAsync((User)null);
            mockMapper.Setup(s => s.Map<User>(newUser)).Returns(user);

            var service = CreateService(
                mockRepository: mockRepository,
                mockMapper: mockMapper,
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            await service.Create(newUser);

            mockFactory.Verify(v => v.Created(user), Times.Once, "Should return the factory's Created method.");
        }

        private CreateUserService CreateService(
            IMock<IContextRepository<IUserContext>> mockRepository = null,
            IMock<IMapper> mockMapper = null,
            IMock<ICreateUserResultFactory> mockFactory = null,
            IMock<IValidator<NewUser>> mockValidator = null)
        {
            mockRepository ??= new Mock<IContextRepository<IUserContext>>();
            mockMapper ??= new Mock<IMapper>();
            mockFactory ??= new Mock<ICreateUserResultFactory>();
            mockValidator ??= new Mock<IValidator<NewUser>>();

            return new CreateUserService(
                mockRepository.Object,
                mockMapper.Object,
                mockFactory.Object,
                mockValidator.Object);
        }
    }
}