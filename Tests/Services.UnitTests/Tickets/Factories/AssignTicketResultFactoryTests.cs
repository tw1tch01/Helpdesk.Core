using System;
using AutoFixture;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Factories.AssignTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Factories
{
    [TestFixture]
    public class AssignTicketResultFactoryTests
    {
        private readonly IFixture _fixture;
        private AssignTicketResultFactory _factory;

        public AssignTicketResultFactoryTests()
        {
            _fixture = new Fixture();

            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public void Setup()
        {
            _factory = new AssignTicketResultFactory();
        }

        [Test]
        public void Assigned()
        {
            var ticket = _fixture.Create<Ticket>();
            var result = _factory.Assigned(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketAssignResult.Assigned, result.Result, $"Should equal {TicketAssignResult.Assigned}.");
                Assert.AreEqual(ResultMessages.Assigned, result.Message, $"Should equal {nameof(ResultMessages.Assigned)} message.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal ticket's TicketId.");
                Assert.AreEqual(ticket.AssignedUserGuid, result.UserGuid, "Should equal ticket's AssignedUserGuid.");
                Assert.IsNull(result.ResolvedBy, "Should be null.");
                Assert.IsNull(result.ResolvedOn, "Should be null.");
                Assert.IsNull(result.ClosedBy, "Should be null.");
                Assert.IsNull(result.ClosedOn, "Should be null.");
                Assert.IsNull(result.Workflow, "Should be null.");
            });
        }

        [Test]
        public void TicketAlreadyClosed()
        {
            var ticket = _fixture.Create<Ticket>();
            var result = _factory.TicketAlreadyClosed(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketAssignResult.TicketAlreadyClosed, result.Result, $"Should equal {TicketAssignResult.TicketAlreadyClosed}.");
                Assert.AreEqual(ResultMessages.TicketAlreadyClosed, result.Message, $"Should equal {nameof(ResultMessages.TicketAlreadyClosed)} message.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal ticket's TicketId.");
                Assert.AreEqual(ticket.AssignedUserGuid, result.UserGuid, "Should equal ticket's AssignedUserGuid.");
                Assert.IsNull(result.ResolvedBy, "Should be null.");
                Assert.IsNull(result.ResolvedOn, "Should be null.");
                Assert.AreEqual(ticket.ClosedBy, result.ClosedBy, "Should equal ticket's ClosedBy.");
                Assert.AreEqual(ticket.ClosedOn, result.ClosedOn, "Should equal ticket's ClosedOn.");
                Assert.IsNull(result.Workflow, "Should be null.");
            });
        }

        [Test]
        public void TicketAlreadyResolved()
        {
            var ticket = _fixture.Create<Ticket>();
            var result = _factory.TicketAlreadyResolved(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketAssignResult.TicketAlreadyResolved, result.Result, $"Should equal {TicketAssignResult.TicketAlreadyResolved}.");
                Assert.AreEqual(ResultMessages.TicketAlreadyResolved, result.Message, $"Should equal {nameof(ResultMessages.TicketAlreadyResolved)} message.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal ticket's TicketId.");
                Assert.AreEqual(ticket.AssignedUserGuid, result.UserGuid, "Should equal ticket's AssignedUserGuid.");
                Assert.AreEqual(ticket.ResolvedBy, result.ResolvedBy, "Should equal ticket's ResolvedBy.");
                Assert.AreEqual(ticket.ResolvedOn, result.ResolvedOn, "Should equal ticket's ResolvedOn.");
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
                Assert.AreEqual(TicketAssignResult.TicketNotFound, result.Result, $"Should equal {TicketAssignResult.TicketNotFound}.");
                Assert.AreEqual(ResultMessages.TicketNotFound, result.Message, $"Should equal {nameof(ResultMessages.TicketNotFound)} message.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal ticketId.");
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
            var mockWorkflow = new Mock<IWorkflowProcess>();
            var result = _factory.WorkflowFailed(ticketId, userGuid, mockWorkflow.Object);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketAssignResult.WorkflowFailed, result.Result, $"Should equal {TicketAssignResult.WorkflowFailed}.");
                Assert.AreEqual(ResultMessages.WorkflowFailed, result.Message, $"Should equal {nameof(ResultMessages.WorkflowFailed)} message.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal ticketId.");
                Assert.AreEqual(userGuid, result.UserGuid, "Should equal userGuid.");
                Assert.IsNull(result.ResolvedBy, "Should be null.");
                Assert.IsNull(result.ResolvedOn, "Should be null.");
                Assert.IsNull(result.ClosedBy, "Should be null.");
                Assert.IsNull(result.ClosedOn, "Should be null.");
                Assert.AreEqual(mockWorkflow.Object, result.Workflow, "Should equal workflow.");
            });
        }
    }
}