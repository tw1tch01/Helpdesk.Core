using System;
using AutoFixture;
using Helpdesk.Domain.Users;
using Helpdesk.Services.Users.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Specifications
{
    [TestFixture]
    public class GetUserByUsernameTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenUsernameDoesNotMatchValue_ReturnsFalse()
        {
            var username = _fixture.Create<string>();
            var user = new User
            {
                Username = _fixture.Create<string>()
            };
            var spec = new GetUserByUsername(username);

            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsFalse(satisfied, "Should return false.");
        }

        [Test]
        public void IsSatisfiedBy_WhenUsernameMatchesValue_ReturnsTrue()
        {
            var username = _fixture.Create<string>();
            var user = new User
            {
                Username = username
            };
            var spec = new GetUserByUsername(username);

            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsTrue(satisfied, "Should return true.");
        }
    }
}