using AutoFixture;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.TicketLinks.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.TicketLinks.Specifications
{
    [TestFixture]
    public class GetLinkedTicketsByIdTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenTicketLinkFromTicketIdDoesNotMatchParameter_ReturnsFalse()
        {
            var fromTicketId = _fixture.Create<int>();
            var toTicketId = _fixture.Create<int>();
            var ticketLink = new TicketLink
            {
                FromTicketId = _fixture.Create<int>(),
                ToTicketId = toTicketId
            };
            var spec = new GetLinkedTicketsById(fromTicketId, toTicketId);
            var result = spec.IsSatisfiedBy(ticketLink);
            Assert.IsFalse(result, "Should return false if fromTicketId does not match.");
        }

        [Test]
        public void IsSatisfiedBy_WhenTicketLinkToTicketIdDoesNotMatchParameter_ReturnsFalse()
        {
            var fromTicketId = _fixture.Create<int>();
            var toTicketId = _fixture.Create<int>();
            var ticketLink = new TicketLink
            {
                FromTicketId = fromTicketId,
                ToTicketId = _fixture.Create<int>()
            };
            var spec = new GetLinkedTicketsById(fromTicketId, toTicketId);
            var result = spec.IsSatisfiedBy(ticketLink);
            Assert.IsFalse(result, "Should return false when toTicketId does not match.");
        }

        [Test]
        public void IsSatisfiedBy_WhenTicketLinkFromTicketIdAndToTicketIdDoNotMatchParameters_ReturnsFalse()
        {
            var fromTicketId = _fixture.Create<int>();
            var toTicketId = _fixture.Create<int>();
            var ticketLink = new TicketLink
            {
                FromTicketId = _fixture.Create<int>(),
                ToTicketId = _fixture.Create<int>()
            };
            var spec = new GetLinkedTicketsById(fromTicketId, toTicketId);
            var result = spec.IsSatisfiedBy(ticketLink);
            Assert.IsFalse(result, "Should return false when neither fromTicketId or toTicketId do not match parameters.");
        }

        [Test]
        public void IsSatisfiedBy_WhenTicketLinkFromTicketIdAndToTicketIdMatchParameters_ReturnsTrue()
        {
            var fromTicketId = _fixture.Create<int>();
            var toTicketId = _fixture.Create<int>();
            var ticketLink = new TicketLink
            {
                FromTicketId = fromTicketId,
                ToTicketId = toTicketId
            };
            var spec = new GetLinkedTicketsById(fromTicketId, toTicketId);
            var result = spec.IsSatisfiedBy(ticketLink);
            Assert.IsTrue(result, "Should return true when fromTicketId and toTicketId match parameters.");
        }
    }
}