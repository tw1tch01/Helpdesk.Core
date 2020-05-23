using System;
using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Commands.UnassignTicket;
using Helpdesk.Services.Tickets.Events.UnassignTicket;
using Helpdesk.Services.Tickets.Factories.UnassignTicket;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class UnassignTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task UnassignUser_VerifyThatSingleAsyncForGetTicketByIdIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockContext: mockContext);

            await service.UnassignUser(ticketId, It.IsAny<Guid>());

            mockContext.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t._ticketId == ticketId)), Times.Once, "Should call the context's SingleAsync method exactly once for GetTicketById.");
        }

        [Test]
        public async Task UnassignUser_WhenTicketRecordIsNull_VerifyFactoryTicketNotFoundIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IUnassignTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.UnassignUser(ticketId, It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task UnassignUser_BeforeTicketIsUnassigned_VerifyBeforeTicketUnassignedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketUnassignedWorkflow>())).ReturnsAsync(new BeforeTicketUnassignedWorkflow(ticketId, userGuid));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.UnassignUser(ticketId, userGuid);

            mockWorkflowService.Verify(v => v.Process(It.Is<BeforeTicketUnassignedWorkflow>(w => w.TicketId == ticketId && w.UserGuid == userGuid)), Times.Once, "Should call the workflow service's Process method for BeforeTicketUnassignedWorkflow.");
        }

        [Test]
        public async Task UnassignUser_WhenBeforeTicketUnassignedWorkflowIsNotSuccessful_VerifyFactoryWorkflowFailedIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockTicket = new Mock<Ticket>();
            var mockBeforeTicketUnassignedWorkflow = new Mock<BeforeTicketUnassignedWorkflow>(It.IsAny<int>(), It.IsAny<Guid>());
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IUnassignTicketResultFactory>();

            mockBeforeTicketUnassignedWorkflow.SetupGet(s => s.Result).Returns(WorkflowResult.Failed);
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketUnassignedWorkflow>())).ReturnsAsync(mockBeforeTicketUnassignedWorkflow.Object);

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.UnassignUser(ticketId, userGuid);

            mockFactory.Verify(v => v.WorkflowFailed(ticketId, userGuid, mockBeforeTicketUnassignedWorkflow.Object), Times.Once, "Should return the factory's WorkflowFailed method.");
        }

        [Test]
        public async Task UnassignUser_WhenTicketCanBeUnassigned_VerifyTicketUnassignMethodIsCalled()
        {
            var userGuid = _fixture.Create<Guid>();
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketUnassignedWorkflow>())).ReturnsAsync(new BeforeTicketUnassignedWorkflow(It.IsAny<int>(), userGuid));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.UnassignUser(It.IsAny<int>(), userGuid);

            mockTicket.Verify(v => v.UnassignUser(), Times.Once, "Should call the ticket's Unassign method.");
        }

        [Test]
        public async Task UnassignUser_WhenTicketIsUnassigned_VerifySaveAsyncIsCalled()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketUnassignedWorkflow>())).ReturnsAsync(new BeforeTicketUnassignedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.UnassignUser(It.IsAny<int>(), It.IsAny<Guid>());

            mockContext.Verify(v => v.SaveAsync(), Times.Once, "Should call the context's SaveAsync.");
        }

        [Test]
        public async Task UnassignUser_WhenTicketIsUnassigned_VerifyTickeUnassignedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            var ticket = new Ticket();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketUnassignedWorkflow>())).ReturnsAsync(new BeforeTicketUnassignedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.UnassignUser(ticketId, userGuid);

            mockWorkflowService.Verify(v => v.Process(It.Is<TicketUnassignedWorkflow>(w => w.TicketId == ticketId && w.UserGuid == userGuid)), Times.Once, "Should process a new TicketUnassignedWorkflow.");
        }

        [Test]
        public async Task UnassignUser_WhenTicketIsUnassigned_VerifyTickeUnassignedNotificationIsQueued()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockNotificationService = new Mock<INotificationService>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            var ticket = new Ticket();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketUnassignedWorkflow>())).ReturnsAsync(new BeforeTicketUnassignedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockContext: mockContext,
                mockNotificationService: mockNotificationService,
                mockWorkflowService: mockWorkflowService);

            await service.UnassignUser(ticketId, userGuid);

            mockNotificationService.Verify(v => v.Queue(It.Is<TicketUnassignedNotification>(n => n.TicketId == ticketId && n.UserGuid == userGuid)), Times.Once, "Should queue a new TicketUnassignedNotification.");
        }

        [Test]
        public async Task UnassignUser_WhenTicketIsUnassigned_VerifyFactoryUnassignedIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IUnassignTicketResultFactory>();

            var ticket = new Ticket();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketUnassignedWorkflow>())).ReturnsAsync(new BeforeTicketUnassignedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.UnassignUser(ticketId, userGuid);

            mockFactory.Verify(v => v.Unassigned(ticketId, userGuid), Times.Once, "Should return the factory's Unassigned method.");
        }

        private UnassignTicketService CreateService(
            IMock<IContextRepository<ITicketContext>> mockContext = null,
            IMock<INotificationService> mockNotificationService = null,
            IMock<IWorkflowService> mockWorkflowService = null,
            IMock<IUnassignTicketResultFactory> mockFactory = null)
        {
            mockContext ??= new Mock<IContextRepository<ITicketContext>>();
            mockNotificationService ??= new Mock<INotificationService>();
            mockWorkflowService ??= new Mock<IWorkflowService>();
            mockFactory ??= new Mock<IUnassignTicketResultFactory>();

            return new UnassignTicketService(
                mockContext.Object,
                mockNotificationService.Object,
                mockWorkflowService.Object,
                mockFactory.Object);
        }
    }
}