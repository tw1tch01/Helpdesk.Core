using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Tests
{
    [TestFixture]
    public class GetAllTicketsTests
    {
        [Test]
        public void IsSatisfiedBy_ReturnsTrue()
        {
            var ticket = new Ticket();
            var spec = new GetAllTickets();
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsTrue(result);
        }
    }
}