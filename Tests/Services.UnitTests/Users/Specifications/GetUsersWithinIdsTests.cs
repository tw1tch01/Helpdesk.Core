using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Helpdesk.Domain.Users;
using Helpdesk.Services.Users.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Specifications
{
    [TestFixture]
    public class GetUsersWithinIdsTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenUserIdIsNotWithinList_ReturnsFalse()
        {
            var user = new User
            {
                UserId = _fixture.Create<int>()
            };
            var ids = _fixture.CreateMany<int>().ToList();
            var spec = new GetUsersWithinIds(ids);

            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsFalse(satisfied, "Should be false.");
        }

        [Test]
        public void IsSatisfiedBy_WhenUserIdIsWithinList_ReturnsTrue()
        {
            var user = new User
            {
                UserId = _fixture.Create<int>()
            };
            var ids = new List<int>
            {
                user.UserId
            };
            var spec = new GetUsersWithinIds(ids);

            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsTrue(satisfied, "Should be true.");
        }
    }
}