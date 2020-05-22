using System;
using AutoFixture;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Events.PauseTicket;
using Helpdesk.Services.Tickets.Factories.PauseTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Factories
{
    public class PauseTicketResultFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();
        private PauseTicketResultFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new PauseTicketResultFactory();
        }

        [Test]
        public void Paused()
        {
            var ticket = new Ticket
            {
                TicketId = _fixture.Create<int>(),
                PausedOn = _fixture.Create<DateTimeOffset>()
            };

            var result = _factory.Paused(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketPauseResult.Paused, result.Result, $"Should return {TicketPauseResult.Paused}.");
                Assert.AreEqual(ResultMessages.Paused, result.Message, $"Should return {ResultMessages.Paused}.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal the passed in ticket's TicketId.");
                Assert.IsNull(result.UserGuid, "Should be null.");
                Assert.IsNull(result.ResolvedOn, "Should be null.");
                Assert.IsNull(result.ResolvedBy, "Should be null.");
                Assert.IsNull(result.ClosedBy, "Should be null.");
                Assert.IsNull(result.ClosedOn, "Should be null.");
                Assert.AreEqual(ticket.PausedOn, result.PausedOn, "Should equal the passed in ticket's PausedOn.");
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
                Assert.AreEqual(TicketPauseResult.TicketAlreadyClosed, result.Result, $"Should return {TicketPauseResult.TicketAlreadyClosed}.");
                Assert.AreEqual(ResultMessages.TicketAlreadyClosed, result.Message, $"Should return {ResultMessages.TicketAlreadyClosed}.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal the passed in ticket's TicketId.");
                Assert.IsNull(result.UserGuid, "Should be null.");
                Assert.IsNull(result.ResolvedOn, "Should be null.");
                Assert.IsNull(result.ResolvedBy, "Should be null.");
                Assert.AreEqual(ticket.ClosedBy, result.ClosedBy, "Should equal the passed in ticket's ClosedBy.");
                Assert.AreEqual(ticket.ClosedOn, result.ClosedOn, "Should equal the passed in ticket's ClosedOn.");
                Assert.IsNull(result.PausedOn, "Should be null.");
                Assert.IsNull(result.Workflow, "Should be null.");
            });
        }

        [Test]
        public void TicketAlreadyPaused()
        {
            var ticket = new Ticket
            {
                TicketId = _fixture.Create<int>(),
                PausedOn = _fixture.Create<DateTimeOffset>()
            };

            var result = _factory.TicketAlreadyPaused(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketPauseResult.TicketAlreadyPaused, result.Result, $"Should return {TicketPauseResult.TicketAlreadyPaused}.");
                Assert.AreEqual(ResultMessages.TicketAlreadyPaused, result.Message, $"Should return {ResultMessages.TicketAlreadyPaused}.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal the passed in ticket's TicketId.");
                Assert.IsNull(result.UserGuid, "Should be null.");
                Assert.IsNull(result.ResolvedOn, "Should be null.");
                Assert.IsNull(result.ResolvedBy, "Should be null.");
                Assert.IsNull(result.ClosedBy, "Should be null.");
                Assert.IsNull(result.ClosedOn, "Should be null.");
                Assert.AreEqual(ticket.PausedOn, result.PausedOn, "Should equal the passed in ticket's PausedOn.");
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
                Assert.AreEqual(TicketPauseResult.TicketAlreadyResolved, result.Result, $"Should return {TicketPauseResult.TicketAlreadyResolved}.");
                Assert.AreEqual(ResultMessages.TicketAlreadyResolved, result.Message, $"Should return {ResultMessages.TicketAlreadyResolved}.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal the passed in ticket's TicketId.");
                Assert.IsNull(result.UserGuid, "Should be null.");
                Assert.AreEqual(ticket.ResolvedBy, result.ResolvedBy, "Should equal the passed in ticket's ResolvedBy.");
                Assert.AreEqual(ticket.ResolvedOn, result.ResolvedOn, "Should equal the passed in ticket's ResolvedOn.");
                Assert.IsNull(result.ClosedOn, "Should be null.");
                Assert.IsNull(result.ClosedBy, "Should be null.");
                Assert.IsNull(result.PausedOn, "Should be null.");
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
                Assert.AreEqual(TicketPauseResult.TicketNotFound, result.Result, $"Should return {TicketPauseResult.TicketNotFound}.");
                Assert.AreEqual(ResultMessages.TicketNotFound, result.Message, $"Should return {ResultMessages.TicketNotFound}.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal the passed in ticket's TicketId.");
                Assert.IsNull(result.UserGuid, "Should be null.");
                Assert.IsNull(result.ResolvedOn, "Should be null.");
                Assert.IsNull(result.ResolvedBy, "Should be null.");
                Assert.IsNull(result.ClosedBy, "Should be null.");
                Assert.IsNull(result.ClosedOn, "Should be null.");
                Assert.IsNull(result.PausedOn, "Should be null.");
                Assert.IsNull(result.Workflow, "Should be null.");
            });
        }

        [Test]
        public void WorkflowFailed()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var workflow = new BeforeTicketPausedWorkflow(ticketId, userGuid);

            var result = _factory.WorkflowFailed(ticketId, userGuid, workflow);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketPauseResult.WorkflowFailed, result.Result, $"Should return {TicketPauseResult.WorkflowFailed}.");
                Assert.AreEqual(ResultMessages.WorkflowFailed, result.Message, $"Should return {ResultMessages.WorkflowFailed}.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal the passed in ticketId.");
                Assert.AreEqual(userGuid, result.UserGuid, "Should equal the passed in userGuid.");
                Assert.IsNull(result.ResolvedOn, "Should be null.");
                Assert.IsNull(result.ResolvedBy, "Should be null.");
                Assert.IsNull(result.ClosedOn, "Should be null.");
                Assert.IsNull(result.ClosedBy, "Should be null.");
                Assert.IsNull(result.PausedOn, "Should be null.");
                Assert.AreEqual(workflow, result.Workflow, "Should equal the passed in workflow.");
            });
        }
    }
}