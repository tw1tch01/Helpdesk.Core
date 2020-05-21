using System.Linq;
using AutoFixture;
using FluentValidation.Results;
using Helpdesk.Domain.Entities;
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
        public void ClientNotFound_SetsNecessaryProperties()
        {
            var clientId = _fixture.Create<int>();
            var result = _factory.ClientNotFound(clientId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketOpenResult.ClientNotFound, result.Result, $"Should be {TicketOpenResult.ClientNotFound}.");
                Assert.AreEqual(ResultMessages.ClientNotFound, result.Message, $"Should return the {nameof(ResultMessages.ClientNotFound)} message.");
                Assert.IsNull(result.TicketId, "Should be null.");
                Assert.AreEqual(clientId, result.ClientId, "Should equal clientId passed through.");
                Assert.IsNull(result.ProjectId, "Should be null.");
                Assert.IsNull(result.ValidationFailures, "Should be null.");
            });
        }

        [Test]
        public void Opened_SetsNecessaryProperties()
        {
            var ticket = new Ticket
            {
                TicketId = _fixture.Create<int>(),
                ClientId = _fixture.Create<int>(),
            };
            var result = _factory.Opened(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketOpenResult.Opened, result.Result, $"Should be {TicketOpenResult.Opened}.");
                Assert.AreEqual(ResultMessages.Opened, result.Message, $"Should return the {nameof(ResultMessages.Opened)} message.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal ticketId passed through.");
                Assert.AreEqual(ticket.ClientId, result.ClientId, "Should equal clientId passed through.");
                Assert.IsNull(result.ValidationFailures, "Should be null.");
            });
        }

        [Test]
        public void ProjectInaccessible_SetsNecessaryProperties()
        {
            var clientId = _fixture.Create<int>();
            var projectId = _fixture.Create<int>();
            var result = _factory.ProjectInaccessible(clientId, projectId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketOpenResult.ProjectInaccessible, result.Result, $"Should be {TicketOpenResult.ProjectInaccessible}.");
                Assert.AreEqual(ResultMessages.ProjectInaccessible, result.Message, $"Should return the {nameof(ResultMessages.ProjectInaccessible)} message.");
                Assert.IsNull(result.TicketId, "Should be null.");
                Assert.AreEqual(clientId, result.ClientId, "Should equal clientId passed through.");
                Assert.AreEqual(projectId, result.ProjectId, "Should equal projectId passed through.");
                Assert.IsNull(result.ValidationFailures, "Should be null.");
            });
        }

        [Test]
        public void ProjectNotFound_SetsNecessaryProperties()
        {
            var projectId = _fixture.Create<int>();
            var result = _factory.ProjectNotFound(projectId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketOpenResult.ProjectNotFound, result.Result, $"Should be {TicketOpenResult.ProjectNotFound}.");
                Assert.AreEqual(ResultMessages.ProjectNotFound, result.Message, $"Should return the {nameof(ResultMessages.ProjectNotFound)} message.");
                Assert.IsNull(result.TicketId, "Should be null.");
                Assert.IsNull(result.ClientId, "Should be null.");
                Assert.AreEqual(projectId, result.ProjectId, "Should equal projectId passed through.");
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
                Assert.IsNull(result.ClientId, "Should be null.");
                Assert.IsNull(result.ProjectId, "Should be null.");
                Assert.IsNotEmpty(result.ValidationFailures, "Should not be empty.");
            });
        }
    }
}