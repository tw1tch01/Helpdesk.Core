using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Commands.ReopenTicket;
using Helpdesk.Services.Tickets.Events.ReopenTicket;
using Helpdesk.Services.Tickets.Factories.ReopenTicket;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class ReopenTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task Reopen_VerifySingleAsyncForGetTicketByIdIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockRepository: mockRepository);

            await service.Reopen(ticketId, It.IsAny<int>());

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t._ticketId == ticketId)), Times.Once, "Should call the repository's SingleAsync exactly once for GetTicketById.");
        }

        [Test]
        public async Task Reopen_WhenTicketRecordIsNull_VerifyFactoryTicketNotFoundIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IReopenTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Reopen(ticketId, It.IsAny<int>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task Reopen_BeforeTicketIsReopened_VerifyBeforeTicketReopenedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketReopenedWorkflow>())).ReturnsAsync(new BeforeTicketReopenedWorkflow(ticketId, userId));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Reopen(ticketId, userId);

            mockWorkflowService.Verify(v => v.Process(It.Is<BeforeTicketReopenedWorkflow>(w => w.TicketId == ticketId && w.UserId == userId)), Times.Once, "Should call the workflow services's Process method for BeforeTicketReopenedWorkflow.");
        }

        [Test]
        public async Task Reopen_WhenBeforeTicketReopenedWorkflowIsNotSuccessful_VerifyFactoryWorkflowFailedIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var mockWorkflow = new Mock<BeforeTicketReopenedWorkflow>(ticketId, userId);
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IReopenTicketResultFactory>();

            mockWorkflow.Setup(a => a.Result).Returns(WorkflowResult.Failed);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketReopenedWorkflow>())).ReturnsAsync(mockWorkflow.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.Reopen(ticketId, userId);

            mockFactory.Verify(v => v.WorkflowFailed(ticketId, userId, mockWorkflow.Object), Times.Once, "Should return the factory's WorkflowFailed method.");
        }

        [Test]
        public async Task Reopen_VerifyTicketReopenIsCalled()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketReopenedWorkflow>())).ReturnsAsync(new BeforeTicketReopenedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Reopen(It.IsAny<int>(), It.IsAny<int>());

            mockTicket.Verify(v => v.Reopen(), Times.Once, "Should call the repository's Reopen method.");
        }

        [Test]
        public async Task Reopen_VerifySaveAsyncIsCalled()
        {
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketReopenedWorkflow>())).ReturnsAsync(new BeforeTicketReopenedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Reopen(It.IsAny<int>(), It.IsAny<int>());

            mockRepository.Verify(v => v.SaveAsync(), Times.Once, "Should call the repository's SaveAsync method.");
        }

        [Test]
        public async Task Reopen_WhenTicketIsReopened_VerifyTicketReopenedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketReopenedWorkflow>())).ReturnsAsync(new BeforeTicketReopenedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Reopen(ticketId, userId);

            mockWorkflowService.Verify(v => v.Process(It.Is<TicketReopenedWorkflow>(t => t.TicketId == ticketId && t.UserId == userId)), Times.Once, "Should call the workflow service's Process method for TicketReopenedWorkflow.");
        }

        [Test]
        public async Task Reopen_WhenTicketIsDeleted_VerifyTicketReopenedNotificationIsQueued()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockNotificationService = new Mock<INotificationService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketReopenedWorkflow>())).ReturnsAsync(new BeforeTicketReopenedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockNotificationService: mockNotificationService,
                mockWorkflowService: mockWorkflowService);

            await service.Reopen(ticketId, userId);

            mockNotificationService.Verify(v => v.Queue(It.Is<TicketReopenedNotification>(t => t.TicketId == ticketId && t.UserId == userId)), Times.Once, "Should call the notification service's Queue method for TicketReopenedNotification.");
        }

        [Test]
        public async Task Reopen_WhenTicketIsReopened_VerifyFactoryReopenedIsReturned()
        {
            var ticket = new Ticket();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IReopenTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketReopenedWorkflow>())).ReturnsAsync(new BeforeTicketReopenedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.Reopen(It.IsAny<int>(), It.IsAny<int>());

            mockFactory.Verify(v => v.Reopened(ticket), Times.Once, "Should return the factoy's Reopened method.");
        }

        private ReopenTicketService CreateService(
            Mock<IContextRepository<ITicketContext>> mockRepository = null,
            Mock<INotificationService> mockNotificationService = null,
            Mock<IWorkflowService> mockWorkflowService = null,
            Mock<IReopenTicketResultFactory> mockFactory = null)
        {
            mockRepository ??= new Mock<IContextRepository<ITicketContext>>();
            mockNotificationService ??= new Mock<INotificationService>();
            mockWorkflowService ??= new Mock<IWorkflowService>();
            mockFactory ??= new Mock<IReopenTicketResultFactory>();

            return new ReopenTicketService(
                mockRepository.Object,
                mockNotificationService.Object,
                mockWorkflowService.Object,
                mockFactory.Object);
        }
    }
}