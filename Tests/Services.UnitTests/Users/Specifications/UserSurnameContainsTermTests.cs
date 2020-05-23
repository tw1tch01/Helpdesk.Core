using AutoFixture;
using Helpdesk.Domain.Users;
using Helpdesk.Services.Users.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Specifications
{
    [TestFixture]
    public class UserSurnameContainsTermTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenTermIsEmpty_ReturnsTrue()
        {
            var term = string.Empty;
            var user = _fixture.Create<User>();
            var spec = new UserSurnameContainsTerm(term);
            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsTrue(satisfied, "Should be true.");
        }

        [Test]
        public void IsSatisfiedBy_WhenSurnameDoesNotContainTerm_ReturnsFalse()
        {
            var user = new User
            {
                Surname = _fixture.Create<string>()
            };
            var term = _fixture.Create<string>().Substring(6, 10);
            var spec = new UserSurnameContainsTerm(term);
            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsFalse(satisfied, "Should be true.");
        }

        [Test]
        public void IsSatisfiedBy_WhenSurnameDoesContainTerm_ReturnsTrue()
        {
            var user = new User
            {
                Surname = _fixture.Create<string>()
            };
            var term = user.Surname.Substring(6, 10);
            var spec = new UserSurnameContainsTerm(term);
            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsTrue(satisfied, "Should be true.");
        }
    }
}