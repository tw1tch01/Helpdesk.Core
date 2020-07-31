using System;
using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Commands.StartTicket;
using Helpdesk.Services.Tickets.Events.StartTicket;
using Helpdesk.Services.Tickets.Factories.StartTicket;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class StartTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task Start_VerifySingleAsyncforGetTicketByIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockRepository: mockRepository);

            await service.Start(ticketId, It.IsAny<Guid>());

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t._ticketId == ticketId)), Times.Once, "Should call the repository's SingleAsync exactly once for GetTicketById.");
        }

        [Test]
        public async Task Start_WhenTicketRecordINull_ReturnsFactoryTicketNotFound()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IStartTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Start(ticketId, It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task Start_WhenTicketIsResolved_ReturnsFactoryTicketAlreadyResolved()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IStartTicketResultFactory>();

            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Resolved);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Start(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyResolved(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyResolved method.");
        }

        [Test]
        public async Task Start_WhenTicketIsClosed_ReturnsFactoryTicketAlreadyClosed()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IStartTicketResultFactory>();

            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Closed);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Start(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyClosed(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyClosed method.");
        }

        [Test]
        public async Task Start_WhenTicketIsInProgress_ReturnsFactoryTicketAlreadyStarted()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IStartTicketResultFactory>();

            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.InProgress);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Start(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyStarted(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyStarted method.");
        }

        [Test]
        public async Task Start_BeforeTicketIsStarted_VerifyBeforeTicketStartedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketStartedWorkflow>())).ReturnsAsync(new BeforeTicketStartedWorkflow(ticketId, userGuid));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Start(ticketId, userGuid);

            mockWorkflowService.Verify(v => v.Process(It.Is<BeforeTicketStartedWorkflow>(w => w.TicketId == ticketId && w.UserGuid == userGuid)), Times.Once, "Should call the workflow flow's Process method for BeforeTicketStartedWorkflow.");
        }

        [Test]
        public async Task Start_WhenBeforeTicketStartedWorkflowIsNotSuccessful_VerifyFactoryWorkflowFailedIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockWorkflow = new Mock<BeforeTicketStartedWorkflow>(It.IsAny<int>(), It.IsAny<Guid>());
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IStartTicketResultFactory>();

            mockWorkflow.Setup(a => a.Result).Returns(WorkflowResult.Failed);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketStartedWorkflow>())).ReturnsAsync(mockWorkflow.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.Start(ticketId, userGuid);

            mockFactory.Verify(v => v.WorkflowFailed(ticketId, userGuid, mockWorkflow.Object), Times.Once, "Should return the factory's WorkflowFailed method.");
        }

        [Test]
        public async Task Start_VerifyTicketStartedIsCalled()
        {
            var userGuid = It.IsAny<Guid>();
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketStartedWorkflow>())).ReturnsAsync(new BeforeTicketStartedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Start(It.IsAny<int>(), userGuid);

            mockTicket.Verify(v => v.Start(userGuid), Times.Once, "Should call the ticket's Start method.");
        }

        [Test]
        public async Task Start_VerifySaveAsyncIsCalled()
        {
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketStartedWorkflow>())).ReturnsAsync(new BeforeTicketStartedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Start(It.IsAny<int>(), It.IsAny<Guid>());

            mockRepository.Verify(v => v.SaveAsync(), Times.Once, "Should call the repository's SaveAsync method exactly once.");
        }

        [Test]
        public async Task Start_WhenTicketIsStarted_VerifyTicketStartedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketStartedWorkflow>())).ReturnsAsync(new BeforeTicketStartedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Start(ticketId, userGuid);

            mockWorkflowService.Verify(v => v.Process(It.Is<TicketStartedWorkflow>(w => w.TicketId == ticketId && w.UserGuid == userGuid)), Times.Once, "Should call the workflow flow's Process method for TicketStartedWorkflow.");
        }

        [Test]
        public async Task Start_WhenTicketIsStarted_VerifyTicketStartedNotificationIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockNotificationService = new Mock<INotificationService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketStartedWorkflow>())).ReturnsAsync(new BeforeTicketStartedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockNotificationService: mockNotificationService,
                mockWorkflowService: mockWorkflowService);

            await service.Start(ticketId, userGuid);

            mockNotificationService.Verify(v => v.Queue(It.Is<TicketStartedNotification>(n => n.TicketId == ticketId && n.UserGuid == userGuid)), Times.Once, "Should call the notification service's Queue method for TicketStartedNotification.");
        }

        [Test]
        public async Task Start_WhenTicketIsStarted_VerifyFactoryStartedIsReturned()
        {
            var ticket = new Ticket();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IStartTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketStartedWorkflow>())).ReturnsAsync(new BeforeTicketStartedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.Start(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.Started(ticket), Times.Once, "Should return the factory's Started method.");
        }

        private StartTicketService CreateService(
            Mock<IContextRepository<ITicketContext>> mockRepository = null,
            Mock<INotificationService> mockNotificationService = null,
            Mock<IWorkflowService> mockWorkflowService = null,
            Mock<IStartTicketResultFactory> mockFactory = null)
        {
            mockRepository ??= new Mock<IContextRepository<ITicketContext>>();
            mockNotificationService ??= new Mock<INotificationService>();
            mockWorkflowService ??= new Mock<IWorkflowService>();
            mockFactory ??= new Mock<IStartTicketResultFactory>();

            return new StartTicketService(
                mockRepository.Object,
                mockNotificationService.Object,
                mockWorkflowService.Object,
                mockFactory.Object);
        }
    }
}