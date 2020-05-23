using AutoFixture;
using Helpdesk.Domain.Users;
using Helpdesk.Services.Users.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Specifications
{
    [TestFixture]
    public class UserNameContainsTermTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenTermIsEmpty_ReturnsTrue()
        {
            var term = string.Empty;
            var user = _fixture.Create<User>();
            var spec = new UserNameContainsTerm(term);
            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsTrue(satisfied, "Should be true.");
        }

        [Test]
        public void IsSatisfiedBy_WhenNameDoesNotContainTerm_ReturnsFalse()
        {
            var user = new User
            {
                Name = _fixture.Create<string>()
            };
            var term = _fixture.Create<string>().Substring(6, 10);
            var spec = new UserNameContainsTerm(term);
            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsFalse(satisfied, "Should be true.");
        }

        [Test]
        public void IsSatisfiedBy_WhenNameDoesContainTerm_ReturnsTrue()
        {
            var user = new User
            {
                Name = _fixture.Create<string>()
            };
            var term = user.Name.Substring(6, 10);
            var spec = new UserNameContainsTerm(term);
            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsTrue(satisfied, "Should be true.");
        }
    }
}