using System.Linq;
using AutoFixture;
using FluentValidation.Results;
using Helpdesk.Domain.Users;
using Helpdesk.Services.Extensions;
using Helpdesk.Services.Users.Factories.CreateUser;
using Helpdesk.Services.Users.Results;
using Helpdesk.Services.Users.Results.Enums;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Factories
{
    [TestFixture]
    public class CreateUserResultFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();
        private CreateUserResultFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new CreateUserResultFactory();
        }

        [Test]
        public void Created()
        {
            var user = new User
            {
                UserId = _fixture.Create<int>(),
                Username = _fixture.Create<string>()
            };
            var result = _factory.Created(user);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(UserCreateResult.Created, result.Result, $"Should equal {UserCreateResult.Created}.");
                Assert.AreEqual(ResultMessages.Created, result.Message, $"Should equal {nameof(ResultMessages.Created)} message.");
                Assert.AreEqual(user.UserId, result.UserId, "Should equal passed through user's UserId.");
                Assert.AreEqual(user.Username, result.Username, "Should equal passed through user's Username.");
                Assert.IsNull(result.ValidationFailures, "Should be null.");
            });
        }

        [Test]
        public void DuplicateUsername()
        {
            var username = _fixture.Create<string>();
            var result = _factory.DuplicateUsername(username);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(UserCreateResult.DuplicateUsername, result.Result, $"Should equal {UserCreateResult.DuplicateUsername}.");
                Assert.AreEqual(ResultMessages.DuplicateUsername, result.Message, $"Should equal {nameof(ResultMessages.DuplicateUsername)} message.");
                Assert.IsNull(result.UserId, "Should be null.");
                Assert.AreEqual(username, result.Username, "Should equal passed through user's Username.");
                Assert.IsNull(result.ValidationFailures, "Should be null.");
            });
        }

        [Test]
        public void ValidationFailure()
        {
            var validationFailures = _fixture.CreateMany<ValidationFailure>().ToList();
            var result = _factory.ValidationFailure(validationFailures);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(UserCreateResult.ValidationFailure, result.Result, $"Should equal {UserCreateResult.ValidationFailure}.");
                Assert.AreEqual(ResultMessages.ValidationFailure, result.Message, $"Should equal {nameof(ResultMessages.ValidationFailure)} message.");
                Assert.IsNull(result.UserId, "Should be null.");
                Assert.IsNull(result.Username, "Should be null.");
                Assert.AreEqual(validationFailures.GroupPropertyWithErrors(), result.ValidationFailures, "Should equal the erorrs with grouped properties.");
            });
        }
    }
}