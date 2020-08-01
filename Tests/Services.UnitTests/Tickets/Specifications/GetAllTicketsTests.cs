using AutoFixture;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Tests
{
    [TestFixture]
    public class GetAllTicketsTests
    {
        private readonly IFixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Test]
        public void IsSatisfiedBy_ReturnsTrue()
        {
            var ticket = _fixture.Create<Ticket>();
            var spec = new GetAllTickets();
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsTrue(result);
        }
    }
}