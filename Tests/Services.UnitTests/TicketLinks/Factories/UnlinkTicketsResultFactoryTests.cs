using AutoFixture;
using Helpdesk.Services.TicketLinks.Factories.UnlinkTickets;
using Helpdesk.Services.TicketLinks.Results;
using Helpdesk.Services.TicketLinks.Results.Enums;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.TicketLinks.Factories
{
    [TestFixture]
    public class UnlinkTicketsResultFactoryTests
    {
        private readonly IFixture _fixture;
        private UnlinkTicketsResultFactory _factory;

        public UnlinkTicketsResultFactoryTests()
        {
            _fixture = new Fixture();

            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public void Setup()
        {
            _factory = new UnlinkTicketsResultFactory();
        }

        [Test]
        public void TicketsNotLinked()
        {
            var fromTicketId = _fixture.Create<int>();
            var toTicketId = _fixture.Create<int>();
            var result = _factory.TicketsNotLinked(fromTicketId, toTicketId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketsUnlinkResult.TicketsNotLinked, result.Result);
                Assert.AreEqual(ResultMessages.TicketsNotLinked, result.Message);
                Assert.AreEqual(fromTicketId, result.FromTicketId);
                Assert.AreEqual(toTicketId, result.ToTicketId);
            });
        }

        [Test]
        public void Unlinked()
        {
            var fromTicketId = _fixture.Create<int>();
            var toTicketId = _fixture.Create<int>();
            var result = _factory.Unlinked(fromTicketId, toTicketId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketsUnlinkResult.Unlinked, result.Result);
                Assert.AreEqual(ResultMessages.Unlinked, result.Message);
                Assert.AreEqual(fromTicketId, result.FromTicketId);
                Assert.AreEqual(toTicketId, result.ToTicketId);
            });
        }
    }
}