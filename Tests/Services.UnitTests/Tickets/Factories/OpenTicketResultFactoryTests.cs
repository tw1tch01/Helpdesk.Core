using System;
using System.Linq;
using AutoFixture;
using FluentValidation.Results;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Factories.OpenTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Factories
{
    [TestFixture]
    public class OpenTicketResultFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();
        private OpenTicketResultFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new OpenTicketResultFactory();
        }

        [Test]
        public void Opened_SetsNecessaryProperties()
        {
            var ticket = new Ticket
            {
                TicketId = _fixture.Create<int>(),
                UserGuid = _fixture.Create<Guid>(),
            };
            var result = _factory.Opened(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketOpenResult.Opened, result.Result, $"Should be {TicketOpenResult.Opened}.");
                Assert.AreEqual(ResultMessages.Opened, result.Message, $"Should return the {nameof(ResultMessages.Opened)} message.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal ticketId passed through.");
                Assert.AreEqual(ticket.UserGuid, result.UserGuid, "Should equal clientId passed through.");
                Assert.IsNull(result.ValidationFailures, "Should be null.");
            });
        }

        [Test]
        public void ValidationFailure_SetsNecessaryProperties()
        {
            var validationFailures = _fixture.CreateMany<ValidationFailure>().ToList();
            var result = _factory.ValidationFailure(validationFailures);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketOpenResult.ValidationFailure, result.Result, $"Should be {TicketOpenResult.ValidationFailure}.");
                Assert.AreEqual(ResultMessages.ValidationFailure, result.Message, $"Should return the {nameof(ResultMessages.ValidationFailure)} message.");
                Assert.IsNull(result.TicketId, "Should be null.");
                Assert.IsNull(result.UserGuid, "Should be null.");
                Assert.IsNotEmpty(result.ValidationFailures, "Should not be empty.");
            });
        }
    }
}