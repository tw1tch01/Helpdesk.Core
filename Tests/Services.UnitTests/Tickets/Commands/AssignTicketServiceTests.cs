using System;
using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Enums;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Commands.AssignTicket;
using Helpdesk.Services.Tickets.Events.AssignTicket;
using Helpdesk.Services.Tickets.Factories.AssignTicket;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class AssignTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task AssignUser_VerifyThatSingleAsyncForGetTicketByIdIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockContext: mockContext);

            await service.AssignUser(ticketId, It.IsAny<Guid>());

            mockContext.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t._ticketId == ticketId)), Times.Once, "Should call the context's SingleAsync method exactly once for GetTicketById.");
        }

        [Test]
        public async Task AssignUser_WhenTicketRecordIsNull_VerifyFactoryTicketNotFoundIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IAssignTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.AssignUser(ticketId, It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task AssignUser_WhenTicketIsResolved_VerifyFactoryTicketAlreadyResolvedIsReturned()
        {
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IAssignTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Resolved);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.AssignUser(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyResolved(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyResolved method.");
        }

        [Test]
        public async Task AssignUser_WhenTicketIsAssigned_VerifyFactoryTicketAlreadyAssignedIsReturned()
        {
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IAssignTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Closed);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.AssignUser(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyClosed(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyAssigned method.");
        }

        [Test]
        public async Task AssignUser_BeforeTicketIsAssigned_VerifyBeforeTicketAssignedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketAssignedWorkflow>())).ReturnsAsync(new BeforeTicketAssignedWorkflow(ticketId, userGuid));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.AssignUser(ticketId, userGuid);

            mockWorkflowService.Verify(v => v.Process(It.Is<BeforeTicketAssignedWorkflow>(w => w.TicketId == ticketId && w.UserGuid == userGuid)), Times.Once, "Should call the workflow service's Process method for BeforeTicketAssignedWorkflow.");
        }

        [Test]
        public async Task AssignUser_WhenBeforeTicketAssignedWorkflowIsNotSuccessful_VerifyFactoryWorkflowFailedIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockTicket = new Mock<Ticket>();
            var mockBeforeTicketAssignedWorkflow = new Mock<BeforeTicketAssignedWorkflow>(It.IsAny<int>(), It.IsAny<Guid>());
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IAssignTicketResultFactory>();

            mockBeforeTicketAssignedWorkflow.SetupGet(s => s.Result).Returns(WorkflowResult.Failed);
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketAssignedWorkflow>())).ReturnsAsync(mockBeforeTicketAssignedWorkflow.Object);

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.AssignUser(ticketId, userGuid);

            mockFactory.Verify(v => v.WorkflowFailed(ticketId, userGuid, mockBeforeTicketAssignedWorkflow.Object), Times.Once, "Should return the factory's WorkflowFailed method.");
        }

        [Test]
        public async Task AssignUser_WhenTicketCanBeAssigned_VerifyTicketAssignMethodIsCalled()
        {
            var userGuid = _fixture.Create<Guid>();
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketAssignedWorkflow>())).ReturnsAsync(new BeforeTicketAssignedWorkflow(It.IsAny<int>(), userGuid));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.AssignUser(It.IsAny<int>(), userGuid);

            mockTicket.Verify(v => v.AssignUser(userGuid), Times.Once, "Should call the ticket's Assign method.");
        }

        [Test]
        public async Task AssignUser_WhenTicketIsAssigned_VerifySaveAsyncIsCalled()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketAssignedWorkflow>())).ReturnsAsync(new BeforeTicketAssignedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.AssignUser(It.IsAny<int>(), It.IsAny<Guid>());

            mockContext.Verify(v => v.SaveAsync(), Times.Once, "Should call the context's SaveAsync.");
        }

        [Test]
        public async Task AssignUser_WhenTicketIsAssigned_VerifyTickeAssignedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            var ticket = new Ticket();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketAssignedWorkflow>())).ReturnsAsync(new BeforeTicketAssignedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.AssignUser(ticketId, userGuid);

            mockWorkflowService.Verify(v => v.Process(It.Is<TicketAssignedWorkflow>(w => w.TicketId == ticketId && w.UserGuid == userGuid)), Times.Once, "Should process a new TicketAssignedWorkflow.");
        }

        [Test]
        public async Task AssignUser_WhenTicketIsAssigned_VerifyTickeAssignedNotificationIsQueued()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockNotificationService = new Mock<INotificationService>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            var ticket = new Ticket();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketAssignedWorkflow>())).ReturnsAsync(new BeforeTicketAssignedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockContext: mockContext,
                mockNotificationService: mockNotificationService,
                mockWorkflowService: mockWorkflowService);

            await service.AssignUser(ticketId, userGuid);

            mockNotificationService.Verify(v => v.Queue(It.Is<TicketAssignedNotification>(n => n.TicketId == ticketId && n.UserGuid == userGuid)), Times.Once, "Should queue a new TicketAssignedNotification.");
        }

        [Test]
        public async Task AssignUser_WhenTicketIsAssigned_VerifyFactoryAssignedIsReturned()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IAssignTicketResultFactory>();

            var ticket = new Ticket();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketAssignedWorkflow>())).ReturnsAsync(new BeforeTicketAssignedWorkflow(It.IsAny<int>(), It.IsAny<Guid>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.AssignUser(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.Assigned(ticket), Times.Once, "Should return the factory's Assigned method.");
        }

        private AssignTicketService CreateService(
            IMock<IContextRepository<ITicketContext>> mockContext = null,
            IMock<INotificationService> mockNotificationService = null,
            IMock<IWorkflowService> mockWorkflowService = null,
            IMock<IAssignTicketResultFactory> mockFactory = null)
        {
            mockContext ??= new Mock<IContextRepository<ITicketContext>>();
            mockNotificationService ??= new Mock<INotificationService>();
            mockWorkflowService ??= new Mock<IWorkflowService>();
            mockFactory ??= new Mock<IAssignTicketResultFactory>();

            return new AssignTicketService(
                mockContext.Object,
                mockNotificationService.Object,
                mockWorkflowService.Object,
                mockFactory.Object);
        }
    }
}