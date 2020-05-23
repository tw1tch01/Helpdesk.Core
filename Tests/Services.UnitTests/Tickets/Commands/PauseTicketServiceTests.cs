using System;
using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Commands.PauseTicket;
using Helpdesk.Services.Tickets.Events.PauseTicket;
using Helpdesk.Services.Tickets.Factories.PauseTicket;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class PauseTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task Pause_VerifySingleAsyncforGetTicketByIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockRepository: mockRepository);

            await service.Pause(ticketId, It.IsAny<Guid>());

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t._ticketId == ticketId)), Times.Once, "Should call the repository's SingleAsync exactly once for GetTicketById.");
        }

        [Test]
        public async Task Pause_WhenTicketRecordINull_ReturnsFactoryTicketNotFound()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IPauseTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Pause(ticketId, It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task Pause_WhenTicketIsResolved_ReturnsFactoryTicketAlreadyResolved()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IPauseTicketResultFactory>();

            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Resolved);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Pause(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyResolved(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyResolved method.");
        }

        [Test]
        public async Task Pause_WhenTicketIsOnHold_ReturnsFactoryTicketAlreadyPaused()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IPauseTicketResultFactory>();

            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.OnHold);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Pause(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyPaused(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyPaused method.");
        }

        [Test]
        public async Task Pause_WhenTicketIsClosed_ReturnsFactoryTicketAlreadyClosed()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IPauseTicketResultFactory>();

            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Closed);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Pause(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyClosed(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyClosed method.");
        }

        [Test]
        public async Task Pause_BeforeTicketIsPaused_VerifyBeforeTicketPausedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketPausedWorkflow>())).ReturnsAsync(new BeforeTicketPausedWorkflow(ticketId, userGuid));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Pause(ticketId, userGuid);

            mockWorkflowService.Verify(v => v.Process(It.Is<BeforeTicketPausedWorkflow>(w => w.TicketId == ticketId && w.UserGuid == userGuid)), Times.Once, "Should call the workflow flow's Process method for BeforeTicketPausedWorkflow.");
        }

        [Test]
        public async Task Pause_WhenBeforeTicketPausedWorkflowIsNotSuccessful_VerifyFactoryWorkflowFailedIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockBeforeTicketPausdWorflow = new Mock<BeforeTicketPausedWorkflow>(It.IsAny<int>(), It.IsAny<Guid>());
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IPauseTicketResultFactory>();

            mockBeforeTicketPausdWorflow.Setup(a => a.Result).Returns(WorkflowResult.Failed);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketPausedWorkflow>())).ReturnsAsync(mockBeforeTicketPausdWorflow.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.Pause(ticketId, userGuid);

            mockFactory.Verify(v => v.WorkflowFailed(ticketId, userGuid, mockBeforeTicketPausdWorflow.Object), Times.Once, "Should return the factory's WorkflowFailed method.");
        }

        [Test]
        public async Task Pause_VerifyTicketPausedIsCalled()
        {
            var userGuid = It.IsAny<Guid>();
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketPausedWorkflow>())).ReturnsAsync(new BeforeTicketPausedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Pause(It.IsAny<int>(), userGuid);

            mockTicket.Verify(v => v.Pause(userGuid), Times.Once, "Should call the ticket's Pause method.");
        }

        [Test]
        public async Task Pause_VerifySaveAsyncIsCalled()
        {
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketPausedWorkflow>())).ReturnsAsync(new BeforeTicketPausedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Pause(It.IsAny<int>(), It.IsAny<Guid>());

            mockRepository.Verify(v => v.SaveAsync(), Times.Once, "Should call the repository's SaveAsync method exactly once.");
        }

        [Test]
        public async Task Pause_WhenTicketIsPaused_VerifyTicketPausedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketPausedWorkflow>())).ReturnsAsync(new BeforeTicketPausedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Pause(ticketId, userGuid);

            mockWorkflowService.Verify(v => v.Process(It.Is<TicketPausedWorkflow>(w => w.TicketId == ticketId && w.UserGuid == userGuid)), Times.Once, "Should call the workflow flow's Process method for TicketPausedWorkflow.");
        }

        [Test]
        public async Task Pause_WhenTicketIsPaused_VerifyTicketPausedNotificationIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockNotificationService = new Mock<INotificationService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketPausedWorkflow>())).ReturnsAsync(new BeforeTicketPausedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockNotificationService: mockNotificationService,
                mockWorkflowService: mockWorkflowService);

            await service.Pause(ticketId, userGuid);

            mockNotificationService.Verify(v => v.Queue(It.Is<TicketPausedNotification>(n => n.TicketId == ticketId && n.UserGuid == userGuid)), Times.Once, "Should call the notification service's Queue method for TicketPausedNotification.");
        }

        [Test]
        public async Task Pause_WhenTicketIsPaused_VerifyFactoryPausedIsReturned()
        {
            var ticket = new Ticket();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IPauseTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketPausedWorkflow>())).ReturnsAsync(new BeforeTicketPausedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.Pause(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.Paused(ticket), Times.Once, "Should return the factory's Paused method.");
        }

        private PauseTicketService CreateService(
            Mock<IContextRepository<ITicketContext>> mockRepository = null,
            Mock<INotificationService> mockNotificationService = null,
            Mock<IWorkflowService> mockWorkflowService = null,
            Mock<IPauseTicketResultFactory> mockFactory = null)
        {
            mockRepository ??= new Mock<IContextRepository<ITicketContext>>();
            mockNotificationService ??= new Mock<INotificationService>();
            mockWorkflowService ??= new Mock<IWorkflowService>();
            mockFactory ??= new Mock<IPauseTicketResultFactory>();

            return new PauseTicketService(
                mockRepository.Object,
                mockNotificationService.Object,
                mockWorkflowService.Object,
                mockFactory.Object);
        }
    }
}