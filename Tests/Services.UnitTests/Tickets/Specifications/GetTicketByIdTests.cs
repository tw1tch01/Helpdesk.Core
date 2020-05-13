using AutoFixture;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Tests
{
    [TestFixture]
    public class GetTicketByIdTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenTicketIdMatchesValue_ReturnsTrue()
        {
            var ticketId = _fixture.Create<int>();
            var ticket = new Ticket
            {
                TicketId = ticketId
            };
            var spec = new GetTicketById(ticketId);
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsTrue(result, "Should return true when Ticket's ticketId matches paramter value.");
        }

        [Test]
        public void IsSatisfiedBy_WhenTicketIdDoesNotMatchValue_ReturnsFalse()
        {
            var ticketId = _fixture.Create<int>();
            var ticket = new Ticket
            {
                TicketId = _fixture.Create<int>()
            };
            var spec = new GetTicketById(ticketId);
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsFalse(result, "Should return false when Ticket's ticketId does not match paramter value.");
        }
    }
}