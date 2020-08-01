using System;
using AutoFixture;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Specifications
{
    [TestFixture]
    public class GetTicketByIdentifierentifierTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenIdentifierMatchesValue_ReturnsTrue()
        {
            var identifier = _fixture.Create<Guid>();
            var ticket = new Ticket
            {
                Identifier = identifier
            };
            var spec = new GetTicketByIdentifier(identifier);
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsTrue(result, "Should return true when Ticket's identifier matches paramter value.");
        }

        [Test]
        public void IsSatisfiedBy_WhenIdentifierDoesNotMatchValue_ReturnsFalse()
        {
            var identifier = _fixture.Create<Guid>();
            var ticket = new Ticket
            {
                Identifier = _fixture.Create<Guid>()
            };
            var spec = new GetTicketByIdentifier(identifier);
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsFalse(result, "Should return false when Ticket's identifier does not match paramter value.");
        }
    }
}