using System;
using AutoFixture;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Events.CloseTicket;
using Helpdesk.Services.Tickets.Factories.CloseTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Factories
{
    [TestFixture]
    public class CloseTicketResultFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();
        private CloseTicketResultFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new CloseTicketResultFactory();
        }

        [Test]
        public void Closed()
        {
            var ticket = new Ticket
            {
                TicketId = _fixture.Create<int>(),
                ClosedBy = _fixture.Create<Guid>(),
                ClosedOn = _fixture.Create<DateTimeOffset>()
            };
            var result = _factory.Closed(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketCloseResult.Closed, result.Result, $"Should be {TicketCloseResult.Closed}.");
                Assert.AreEqual(ResultMessages.Closed, result.Message, $"Should return the {nameof(ResultMessages.Closed)} message.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal ticket's TicketId.");
                Assert.AreEqual(ticket.ClosedBy, result.UserGuid, "Should equal the ClosedBy value.");
                Assert.IsNull(result.ResolvedBy, "Should be null.");
                Assert.IsNull(result.ResolvedOn, "Should be null.");
                Assert.AreEqual(ticket.ClosedBy, result.ClosedBy, "Should equal the ticket's ClosedBy.");
                Assert.AreEqual(ticket.ClosedOn, result.ClosedOn, "Should equal the ticket's ClosedOn.");
                Assert.IsNull(result.Workflow, "Should be null.");
            });
        }

        [Test]
        public void TicketAlreadyClosed()
        {
            var ticket = new Ticket
            {
                TicketId = _fixture.Create<int>(),
                ClosedBy = _fixture.Create<Guid>(),
                ClosedOn = _fixture.Create<DateTimeOffset>()
            };
            var result = _factory.TicketAlreadyClosed(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketCloseResult.TicketAlreadyClosed, result.Result, $"Should be {TicketCloseResult.TicketAlreadyClosed}.");
                Assert.AreEqual(ResultMessages.TicketAlreadyClosed, result.Message, $"Should return the {nameof(ResultMessages.TicketAlreadyClosed)} message.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal ticket's TicketId.");
                Assert.IsNull(result.UserGuid, "Should be null.");
                Assert.IsNull(result.ResolvedBy, "Should be null.");
                Assert.IsNull(result.ResolvedOn, "Should be null.");
                Assert.AreEqual(ticket.ClosedBy, result.ClosedBy, "Should equal the ticket's ClosedBy.");
                Assert.AreEqual(ticket.ClosedOn, result.ClosedOn, "Should equal the ticket's ClosedOn.");
                Assert.IsNull(result.Workflow, "Should be null.");
            });
        }

        [Test]
        public void TicketAlreadyResolved()
        {
            var ticket = new Ticket
            {
                TicketId = _fixture.Create<int>(),
                ResolvedBy = _fixture.Create<Guid>(),
                ResolvedOn = _fixture.Create<DateTimeOffset>()
            };
            var result = _factory.TicketAlreadyResolved(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketCloseResult.TicketAlreadyResolved, result.Result, $"Should be {TicketCloseResult.TicketAlreadyResolved}.");
                Assert.AreEqual(ResultMessages.TicketAlreadyResolved, result.Message, $"Should return the {nameof(ResultMessages.TicketAlreadyClosed)} message.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal ticket's TicketId.");
                Assert.IsNull(result.UserGuid, "Should be null.");
                Assert.AreEqual(result.ResolvedBy, result.ResolvedBy, "Should equal ticket's ResolvedBy.");
                Assert.AreEqual(result.ResolvedOn, result.ResolvedOn, "Should equal ticket's ResvoledOn.");
                Assert.IsNull(result.ClosedBy, "Should be null.");
                Assert.IsNull(result.ClosedOn, "Should be null.");
                Assert.IsNull(result.Workflow, "Should be null.");
            });
        }

        [Test]
        public void TicketNotFound()
        {
            var ticketId = _fixture.Create<int>();
            var result = _factory.TicketNotFound(ticketId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketCloseResult.TicketNotFound, result.Result, $"Should be {TicketCloseResult.TicketNotFound}.");
                Assert.AreEqual(ResultMessages.TicketNotFound, result.Message, $"Should return the {nameof(ResultMessages.TicketNotFound)} message.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal passed in ticketId.");
                Assert.IsNull(result.UserGuid, "Should be null.");
                Assert.IsNull(result.ResolvedBy, "Should be null.");
                Assert.IsNull(result.ResolvedOn, "Should be null.");
                Assert.IsNull(result.ClosedBy, "Should be null.");
                Assert.IsNull(result.ClosedOn, "Should be null.");
                Assert.IsNull(result.Workflow, "Should be null.");
            });
        }

        [Test]
        public void WorkflowFailed()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var beforeTicketClosedWorkflow = new BeforeTicketClosedWorkflow(ticketId, userGuid);
            var result = _factory.WorkflowFailed(ticketId, userGuid, beforeTicketClosedWorkflow);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketCloseResult.WorkflowFailed, result.Result, $"Should be {TicketCloseResult.WorkflowFailed}.");
                Assert.AreEqual(ResultMessages.WorkflowFailed, result.Message, $"Should return the {nameof(ResultMessages.WorkflowFailed)} message.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal passed in ticketId.");
                Assert.AreEqual(userGuid, result.UserGuid, "Should equal passed in userGuid.");
                Assert.IsNull(result.ResolvedBy, "Should be null.");
                Assert.IsNull(result.ResolvedOn, "Should be null.");
                Assert.IsNull(result.ClosedBy, "Should be null.");
                Assert.IsNull(result.ClosedOn, "Should be null.");
                Assert.AreEqual(beforeTicketClosedWorkflow, result.Workflow, "Should equal the failed workflow process.");
            });
        }
    }
}