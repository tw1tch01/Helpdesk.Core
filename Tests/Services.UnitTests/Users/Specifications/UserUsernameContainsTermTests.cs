using AutoFixture;
using Helpdesk.Domain.Users;
using Helpdesk.Services.Users.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Specifications
{
    [TestFixture]
    public class UserUsernameContainsTermTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenTermIsEmpty_ReturnsTrue()
        {
            var term = string.Empty;
            var user = _fixture.Create<User>();
            var spec = new UserUsernameContainsTerm(term);
            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsTrue(satisfied, "Should be true.");
        }

        [Test]
        public void IsSatisfiedBy_WhenUsernameDoesNotContainTerm_ReturnsFalse()
        {
            var user = new User
            {
                Username = _fixture.Create<string>()
            };
            var term = _fixture.Create<string>().Substring(6, 10);
            var spec = new UserUsernameContainsTerm(term);
            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsFalse(satisfied, "Should be true.");
        }

        [Test]
        public void IsSatisfiedBy_WhenUsernameDoesContainTerm_ReturnsTrue()
        {
            var user = new User
            {
                Username = _fixture.Create<string>()
            };
            var term = user.Username.Substring(6, 10);
            var spec = new UserUsernameContainsTerm(term);
            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsTrue(satisfied, "Should be true.");
        }
    }
}