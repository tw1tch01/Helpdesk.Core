using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Helpdesk.Domain.Users;
using Helpdesk.Services.Users.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Users.Specifications
{
    [TestFixture]
    public class GetUsersWithinIdentifiersTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenIdentifierIsNotWithinList_ReturnsFalse()
        {
            var user = new User
            {
                Identifier = _fixture.Create<Guid>()
            };
            var identifiers = _fixture.CreateMany<Guid>().ToList();
            var spec = new GetUsersWithinIdentifiers(identifiers);

            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsFalse(satisfied, "Should be false.");
        }

        [Test]
        public void IsSatisfiedBy_WhenIdentifierIsWithinList_ReturnsTrue()
        {
            var user = new User
            {
                Identifier = _fixture.Create<Guid>()
            };
            var identifiers = new List<Guid>
            {
                user.Identifier
            };
            var spec = new GetUsersWithinIdentifiers(identifiers);

            var satisfied = spec.IsSatisfiedBy(user);

            Assert.IsTrue(satisfied, "Should be true.");
        }
    }
}