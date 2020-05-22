using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentValidation.Results;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.Common;
using Helpdesk.Services.Extensions;
using Helpdesk.Services.Tickets.Factories.UpdateTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Factories
{
    [TestFixture]
    public class UpdateTicketResultFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();
        private UpdateTicketResultFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new UpdateTicketResultFactory();
        }

        [Test]
        public void TicketNotFound()
        {
            var ticketId = _fixture.Create<int>();
            var result = _factory.TicketNotFound(ticketId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketUpdateResult.TicketNotFound, result.Result, $"Should be {TicketUpdateResult.TicketNotFound}.");
                Assert.AreEqual(ResultMessages.TicketNotFound, result.Message, $"Should return the {nameof(ResultMessages.TicketNotFound)} message.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal passed in ticketId.");
                Assert.IsNull(result.ValidationFailures, "Should be null.");
                Assert.IsNull(result.PropertyChanges, "Should be null.");
                Assert.IsNull(result.Workflow, "Should be null.");
            });
        }

        [Test]
        public void Updated()
        {
            var ticket = new Ticket
            {
                TicketId = _fixture.Create<int>()
            };
            var changes = _fixture.CreateMany<KeyValuePair<string, ValueChange>>().ToDictionary(k => k.Key, v => v.Value);
            var result = _factory.Updated(ticket, changes);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketUpdateResult.Updated, result.Result, $"Should be {TicketUpdateResult.Updated}.");
                Assert.AreEqual(ResultMessages.Updated, result.Message, $"Should return the {nameof(ResultMessages.Updated)} message.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal passed through ticket's TicketId .");
                Assert.IsNull(result.ValidationFailures, "Should be null.");
                Assert.AreEqual(changes, result.PropertyChanges, "Should equal passed through changes.");
                Assert.IsNull(result.Workflow, "Should be null.");
            });
        }

        [Test]
        public void ValidationFailure()
        {
            var ticketId = _fixture.Create<int>();
            var validationFailures = _fixture.CreateMany<ValidationFailure>().ToList();
            var result = _factory.ValidationFailure(ticketId, validationFailures);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketUpdateResult.ValidationFailure, result.Result, $"Should be {TicketUpdateResult.ValidationFailure}.");
                Assert.AreEqual(ResultMessages.ValidationFailure, result.Message, $"Should return the {nameof(ResultMessages.ValidationFailure)} message.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal passed through ticketId .");
                Assert.AreEqual(validationFailures.GroupPropertyWithErrors(), result.ValidationFailures, "Should equal the grouped set of the passed through validationFailures.");
                Assert.IsNull(result.PropertyChanges, "Should be null.");
                Assert.IsNull(result.Workflow, "Should be null.");
            });
        }
    }
}