using AutoFixture;
using Helpdesk.Services.Tickets.Events.DeleteTicket;
using Helpdesk.Services.Tickets.Factories.DeleteTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Factories
{
    [TestFixture]
    public class DeleteTicketResultFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();
        private DeleteTicketResultFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new DeleteTicketResultFactory();
        }

        [Test]
        public void Deleted()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();

            var result = _factory.Deleted(ticketId, userId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketDeleteResult.Deleted, result.Result, $"Should be {TicketDeleteResult.Deleted}.");
                Assert.AreEqual(ResultMessages.Deleted, result.Message, $"Should return the {nameof(ResultMessages.Deleted)} message.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal the passed through ticketId.");
                Assert.AreEqual(userId, result.UserId, "Should equal the passed through userId.");
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
                Assert.AreEqual(TicketDeleteResult.TicketNotFound, result.Result, $"Should be {TicketDeleteResult.TicketNotFound}.");
                Assert.AreEqual(ResultMessages.TicketNotFound, result.Message, $"Should return the {nameof(ResultMessages.TicketNotFound)} message.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal the passed through ticketId.");
                Assert.IsNull(result.UserId, "Should be null.");
                Assert.IsNull(result.Workflow, "Should be null.");
            });
        }

        [Test]
        public void WorkflowFailed()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var beforeTicketDeletedWorkflow = new BeforeTicketDeletedWorkflow(ticketId, userId);

            var result = _factory.WorkflowFailed(ticketId, userId, beforeTicketDeletedWorkflow);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketDeleteResult.WorkflowFailed, result.Result, $"Should be {TicketDeleteResult.WorkflowFailed}.");
                Assert.AreEqual(ResultMessages.WorkflowFailed, result.Message, $"Should return the {nameof(ResultMessages.WorkflowFailed)} message.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal the passed through ticketId.");
                Assert.AreEqual(userId, result.UserId, "Should equal the passed through userId.");
                Assert.AreEqual(beforeTicketDeletedWorkflow, result.Workflow, "Should equal the passed through workflow.");
            });
        }
    }
}