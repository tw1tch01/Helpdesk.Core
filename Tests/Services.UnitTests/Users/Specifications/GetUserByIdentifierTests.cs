using System;
using AutoFixture;
using Helpdesk.Domain.Users;
using Helpdesk.Services.Users.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Specifications
{
    [TestFixture]
    public class GetUserByIdentifierTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenUserIdDoesNotMatchValue_ReturnsFalse()
        {
            var userGuid = _fixture.Create<Guid>();
            var user = new User
            {
                Identifier = _fixture.Create<Guid>()
            };
            var spec = new GetUserByIdentifier(userGuid);

            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsFalse(satisfied, "Should return false.");
        }

        [Test]
        public void IsSatisfiedBy_WhenUserIdMatchesValue_ReturnsTrue()
        {
            var userGuid = _fixture.Create<Guid>();
            var user = new User
            {
                Identifier = userGuid
            };
            var spec = new GetUserByIdentifier(userGuid);

            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsTrue(satisfied, "Should return true.");
        }
    }
}