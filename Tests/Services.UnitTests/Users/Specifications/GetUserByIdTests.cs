using AutoFixture;
using Helpdesk.Domain.Users;
using Helpdesk.Services.Users.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Specifications
{
    [TestFixture]
    public class GetUserByIdTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenUserIdDoesNotMatchValue_ReturnsFalse()
        {
            var userId = _fixture.Create<int>();
            var user = new User
            {
                UserId = _fixture.Create<int>()
            };
            var spec = new GetUserById(userId);

            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsFalse(satisfied, "Should return false.");
        }

        [Test]
        public void IsSatisfiedBy_WhenUserIdMatchesValue_ReturnsTrue()
        {
            var userId = _fixture.Create<int>();
            var user = new User
            {
                UserId = userId
            };
            var spec = new GetUserById(userId);

            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsTrue(satisfied, "Should return true.");
        }
    }
}