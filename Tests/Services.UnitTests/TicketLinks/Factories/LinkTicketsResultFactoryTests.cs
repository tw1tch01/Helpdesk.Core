using AutoFixture;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.TicketLinks.Factories.LinkTickets;
using Helpdesk.Services.TicketLinks.Results;
using Helpdesk.Services.TicketLinks.Results.Enums;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.TicketLinks.Factories
{
    [TestFixture]
    public class LinkTicketsResultFactoryTests
    {
        private readonly IFixture _fixture;
        private LinkTicketsResultFactory _factory;

        public LinkTicketsResultFactoryTests()
        {
            _fixture = new Fixture();

            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public void Setup()
        {
            _factory = new LinkTicketsResultFactory();
        }

        [Test]
        public void Linked()
        {
            var ticketLink = _fixture.Create<TicketLink>();
            var result = _factory.Linked(ticketLink);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketsLinkResult.Linked, result.Result);
                Assert.AreEqual(ResultMessages.Linked, result.Message);
                Assert.AreEqual(ticketLink.FromTicketId, result.FromTicketId);
                Assert.AreEqual(ticketLink.ToTicketId, result.ToTicketId);
                Assert.AreEqual(ticketLink.LinkType, result.LinkType);
            });
        }

        [Test]
        public void TicketsAlreadyLinked()
        {
            var ticketLink = _fixture.Create<TicketLink>();
            var result = _factory.TicketsAlreadyLinked(ticketLink);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketsLinkResult.TicketsAlreadyLinked, result.Result);
                Assert.AreEqual(ResultMessages.TicketsAlreadyLinked, result.Message);
                Assert.AreEqual(ticketLink.FromTicketId, result.FromTicketId);
                Assert.AreEqual(ticketLink.ToTicketId, result.ToTicketId);
                Assert.AreEqual(ticketLink.LinkType, result.LinkType);
            });
        }
    }
}