using AutoFixture;
using Helpdesk.Domain.Users;
using Helpdesk.Services.Users.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Specifications
{
    [TestFixture]
    public class GetAllUsersTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_ForAnyUser_ReturnsTrue()
        {
            var user = _fixture.Create<User>();
            var spec = new GetAllUsers();
            var satisfied = spec.IsSatisfiedBy(user);
            Assert.IsTrue(satisfied, "Should be true.");
        }
    }
}