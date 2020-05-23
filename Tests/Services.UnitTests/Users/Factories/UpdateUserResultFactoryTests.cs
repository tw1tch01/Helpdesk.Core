using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentValidation.Results;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Common;
using Helpdesk.Services.Extensions;
using Helpdesk.Services.Users.Factories.UpdateUser;
using Helpdesk.Services.Users.Results;
using Helpdesk.Services.Users.Results.Enums;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Factories
{
    [TestFixture]
    public class UpdateUserResultFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();
        private UpdateUserResultFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new UpdateUserResultFactory();
        }

        [Test]
        public void DuplicateUsername()
        {
            var user = new User
            {
                UserId = _fixture.Create<int>(),
                Username = _fixture.Create<string>()
            };
            var result = _factory.DuplicateUsername(user);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(UserUpdateResult.DuplicateUsername, result.Result, $"Should equal {UserUpdateResult.DuplicateUsername}.");
                Assert.AreEqual(ResultMessages.DuplicateUsername, result.Message, $"Should equal {nameof(ResultMessages.DuplicateUsername)} message.");
                Assert.AreEqual(user.UserId, result.UserId, "Should equal passed through user's UserId.");
                Assert.AreEqual(user.Username, result.Username, "Should equal passed through user's Username.");
                Assert.IsNull(result.ValidationFailures, "Should be null.");
                Assert.IsNull(result.PropertyChanges, "Should be null.");
            });
        }

        [Test]
        public void Updated()
        {
            var user = new User
            {
                UserId = _fixture.Create<int>()
            };
            var changes = _fixture.CreateMany<KeyValuePair<string, ValueChange>>().ToDictionary(p => p.Key, p => p.Value);
            var result = _factory.Updated(user, changes);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(UserUpdateResult.Updated, result.Result, $"Should equal {UserUpdateResult.Updated}.");
                Assert.AreEqual(ResultMessages.Updated, result.Message, $"Should equal {nameof(ResultMessages.Updated)} message.");
                Assert.AreEqual(user.UserId, result.UserId, "Should equal passed through user's UserId.");
                Assert.IsNull(result.Username, "Should be null.");
                Assert.IsNull(result.ValidationFailures, "Should be null.");
                Assert.AreEqual(changes, result.PropertyChanges, "Should equal passed through changes.");
            });
        }

        [Test]
        public void UserNotFound()
        {
            var userId = _fixture.Create<int>();
            var result = _factory.UserNotFound(userId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(UserUpdateResult.UserNotFound, result.Result, $"Should equal {UserUpdateResult.UserNotFound}.");
                Assert.AreEqual(ResultMessages.UserNotFound, result.Message, $"Should equal {nameof(ResultMessages.UserNotFound)} message.");
                Assert.AreEqual(userId, result.UserId, "Should equal passed through user's UserId.");
                Assert.IsNull(result.Username, "Should be null.");
                Assert.IsNull(result.ValidationFailures, "Should be null.");
                Assert.IsNull(result.PropertyChanges, "Should be null.");
            });
        }

        [Test]
        public void ValidationFailure()
        {
            var userId = _fixture.Create<int>();
            var validationFailures = _fixture.CreateMany<ValidationFailure>().ToList();
            var result = _factory.ValidationFailure(userId, validationFailures);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(UserUpdateResult.ValidationFailure, result.Result, $"Should equal {UserUpdateResult.ValidationFailure}.");
                Assert.AreEqual(ResultMessages.ValidationFailure, result.Message, $"Should equal {nameof(ResultMessages.ValidationFailure)} message.");
                Assert.AreEqual(userId, result.UserId, "Should equal the passed through userId.");
                Assert.IsNull(result.Username, "Should be null.");
                Assert.AreEqual(validationFailures.GroupPropertyWithErrors(), result.ValidationFailures, "Should equal the erorrs with grouped properties.");
            });
        }
    }
}