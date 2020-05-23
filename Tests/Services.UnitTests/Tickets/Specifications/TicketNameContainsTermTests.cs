using AutoFixture;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Tests
{
    [TestFixture]
    public class TicketNameContainsTermTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenNameContainsTerm_ReturnsTrue()
        {
            var name = _fixture.Create<string>();
            var ticket = new Ticket
            {
                Name = $"{_fixture.Create<string>()}{name}{_fixture.Create<string>()}"
            };
            var spec = new TicketNameContainsTerm(name);
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsTrue(result, "Should return true when name contains search term.");
        }

        [Test]
        public void IsSatisfiedBy_WhenNameDoesNotContainsTerm_ReturnsFalse()
        {
            var name = _fixture.Create<string>();
            var ticket = new Ticket
            {
                Name = $"{_fixture.Create<string>()}{_fixture.Create<string>()}{_fixture.Create<string>()}"
            };
            var spec = new TicketNameContainsTerm(name);
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsFalse(result, "Should return false when name does not contain search term.");
        }
    }
}