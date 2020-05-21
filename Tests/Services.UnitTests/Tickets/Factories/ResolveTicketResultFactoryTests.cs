using System;
using AutoFixture;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Events.ResolveTicket;
using Helpdesk.Services.Tickets.Factories.ResolveTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Factories
{
    [TestFixture]
    public class ResolveTicketResultFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();
        private ResolveTicketResultFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new ResolveTicketResultFactory();
        }

        [Test]
        public void Resolve()
        {
            var ticket = new Ticket
            {
                TicketId = _fixture.Create<int>(),
                ResolvedBy = _fixture.Create<Guid>(),
                ResolvedOn = _fixture.Create<DateTimeOffset>()
            };
            var result = _factory.Resolved(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketResolveResult.Resolved, result.Result, $"Should be {TicketResolveResult.Resolved}.");
                Assert.AreEqual(ResultMessages.Resolved, result.Message, $"Should return the {nameof(ResultMessages.Resolved)} message.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal ticket's TicketId.");
                Assert.AreEqual(ticket.ResolvedBy, result.UserGuid, "Should equal the ClosedBy value.");
                Assert.AreEqual(ticket.ResolvedBy, result.ResolvedBy, "Should equal the ticket's ClosedBy.");
                Assert.AreEqual(ticket.ResolvedOn, result.ResolvedOn, "Should equal the ticket's ClosedOn.");
                Assert.IsNull(result.ClosedBy, "Should be null.");
                Assert.IsNull(result.ClosedOn, "Should be null.");
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
                Assert.AreEqual(TicketResolveResult.TicketAlreadyClosed, result.Result, $"Should be {TicketResolveResult.TicketAlreadyClosed}.");
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
                Assert.AreEqual(TicketResolveResult.TicketAlreadyResolved, result.Result, $"Should be {TicketResolveResult.TicketAlreadyResolved}.");
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
                Assert.AreEqual(TicketResolveResult.TicketNotFound, result.Result, $"Should be {TicketResolveResult.TicketNotFound}.");
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
            var beforeTicketResolvedWorkflow = new BeforeTicketResolvedWorkflow(ticketId, userGuid);
            var result = _factory.WorkflowFailed(ticketId, userGuid, beforeTicketResolvedWorkflow);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketResolveResult.WorkflowFailed, result.Result, $"Should be {TicketResolveResult.WorkflowFailed}.");
                Assert.AreEqual(ResultMessages.WorkflowFailed, result.Message, $"Should return the {nameof(ResultMessages.WorkflowFailed)} message.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal passed in ticketId.");
                Assert.AreEqual(userGuid, result.UserGuid, "Should equal passed in userGuid.");
                Assert.IsNull(result.ResolvedBy, "Should be null.");
                Assert.IsNull(result.ResolvedOn, "Should be null.");
                Assert.IsNull(result.ClosedBy, "Should be null.");
                Assert.IsNull(result.ClosedOn, "Should be null.");
                Assert.AreEqual(beforeTicketResolvedWorkflow, result.Workflow, "Should equal the failed workflow process.");
            });
        }
    }
}