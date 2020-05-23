using AutoFixture;
using Helpdesk.Services.Users.Factories.DeleteUser;
using Helpdesk.Services.Users.Results;
using Helpdesk.Services.Users.Results.Enums;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Factories
{
    [TestFixture]
    public class DeleteUserResultFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();
        private DeleteUserResultFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new DeleteUserResultFactory();
        }

        [Test]
        public void Delete()
        {
            var userId = _fixture.Create<int>();
            var result = _factory.Deleted(userId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(UserDeleteResult.Deleted, result.Result, $"Should equal {UserDeleteResult.Deleted}.");
                Assert.AreEqual(ResultMessages.Deleted, result.Message, $"Should equal {nameof(ResultMessages.Deleted)} message.");
                Assert.AreEqual(userId, result.UserId, "Should equal passed through user's UserId.");
            });
        }

        [Test]
        public void UserNotFound()
        {
            var userId = _fixture.Create<int>();
            var result = _factory.UserNotFound(userId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(UserDeleteResult.UserNotFound, result.Result, $"Should equal {UserDeleteResult.UserNotFound}.");
                Assert.AreEqual(ResultMessages.UserNotFound, result.Message, $"Should equal {nameof(ResultMessages.UserNotFound)} message.");
                Assert.AreEqual(userId, result.UserId, "Should equal passed through user's UserId.");
            });
        }
    }
}