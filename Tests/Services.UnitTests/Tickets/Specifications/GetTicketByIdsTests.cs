using System.Collections.Generic;
using AutoFixture;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Tests
{
    [TestFixture]
    public class GetTicketByIdsTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenValueContainsTicketId_ReturnsTrue()
        {
            var ticketId = _fixture.Create<int>();
            var ticket = new Ticket
            {
                TicketId = ticketId
            };
            var spec = new GetTicketByIds(new List<int> { ticketId });
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsTrue(result, "Should return true when ticketId is within the value.");
        }

        [Test]
        public void IsSatisfiedBy_WhenValueDoesNotContainTicketId_ReturnsFalse()
        {
            var ticketId = _fixture.Create<int>();
            var ticket = new Ticket
            {
                TicketId = _fixture.Create<int>()
            };
            var spec = new GetTicketByIds(new List<int> { ticketId });
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsFalse(result, "Should return false when ticketId is not contained within the value.");
        }
    }
}