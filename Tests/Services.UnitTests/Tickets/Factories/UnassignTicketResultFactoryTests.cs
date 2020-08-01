using System;
using AutoFixture;
using Helpdesk.Services.Tickets.Factories.UnassignTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Factories
{
    [TestFixture]
    public class UnassignTicketResultFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();
        private UnassignTicketResultFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new UnassignTicketResultFactory();
        }

        [Test]
        public void Unassigned()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var result = _factory.Unassigned(ticketId, userGuid);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketUnassignResult.Unassigned, result.Result, $"Should equal {TicketUnassignResult.Unassigned}.");
                Assert.AreEqual(ResultMessages.Unassigned, result.Message, $"Should equal {nameof(ResultMessages.Unassigned)} message.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal ticketId.");
                Assert.AreEqual(userGuid, result.UnassignedBy, "Should equal userGuid.");
            });
        }

        [Test]
        public void TicketNotFound()
        {
            var ticketId = _fixture.Create<int>();
            var result = _factory.TicketNotFound(ticketId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketUnassignResult.TicketNotFound, result.Result, $"Should equal {TicketUnassignResult.TicketNotFound}.");
                Assert.AreEqual(ResultMessages.TicketNotFound, result.Message, $"Should equal {nameof(ResultMessages.TicketNotFound)} message.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal ticketId.");
                Assert.IsNull(result.UnassignedBy, "Should be null.");
            });
        }
    }
}