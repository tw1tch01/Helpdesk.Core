using AutoFixture;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Events.ReopenTicket;
using Helpdesk.Services.Tickets.Factories.ReopenTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Factories
{
    [TestFixture]
    public class ReopenTicketResultFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();
        private ReopenTicketResultFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new ReopenTicketResultFactory();
        }

        [Test]
        public void Reopened()
        {
            var ticket = new Ticket
            {
                TicketId = _fixture.Create<int>()
            };

            var result = _factory.Reopened(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketReopenResult.Reopened, result.Result, $"Should be {TicketReopenResult.Reopened}.");
                Assert.AreEqual(ResultMessages.Reopened, result.Message, $"Should return the {nameof(ResultMessages.Reopened)} message.");
                Assert.AreEqual(ticket.TicketId, result.TicketId, "Should equal the passed through ticket's TicketId.");
                //Assert.AreEqual(userId, result.UserId, "Should equal the passed through userId.");
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
                Assert.AreEqual(TicketReopenResult.TicketNotFound, result.Result, $"Should be {TicketReopenResult.TicketNotFound}.");
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
            var workflow = new BeforeTicketReopenedWorkflow(ticketId, userId);

            var result = _factory.WorkflowFailed(ticketId, userId, workflow);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(TicketReopenResult.WorkflowFailed, result.Result, $"Should be {TicketReopenResult.WorkflowFailed}.");
                Assert.AreEqual(ResultMessages.WorkflowFailed, result.Message, $"Should return the {nameof(ResultMessages.WorkflowFailed)} message.");
                Assert.AreEqual(ticketId, result.TicketId, "Should equal the passed through ticketId.");
                Assert.AreEqual(userId, result.UserId, "Should equal the passed through userId.");
                Assert.AreEqual(workflow, result.Workflow, "Should equal the passed through workflow.");
            });
        }
    }
}