using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Commands.CloseTicket;
using Helpdesk.Services.Tickets.Events.CloseTicket;
using Helpdesk.Services.Tickets.Factories.CloseTicket;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class CloseTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task Close_VerifyThatSingleAsyncForGetTicketByIdIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockContext: mockContext);

            await service.Close(ticketId, It.IsAny<int>());

            mockContext.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t._ticketId == ticketId)), Times.Once, "Should call the context's SingleAsync method exactly once for GetTicketById.");
        }

        [Test]
        public async Task Close_WhenTicketRecordIsNull_VerifyFactoryTicketNotFoundIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<ICloseTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Close(ticketId, It.IsAny<int>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task Close_WhenTicketIsResolved_VerifyFactoryTicketAlreadyResolvedIsReturned()
        {
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<ICloseTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Resolved);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Close(It.IsAny<int>(), It.IsAny<int>());

            mockFactory.Verify(v => v.TicketAlreadyResolved(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyResolved method.");
        }

        [Test]
        public async Task Close_WhenTicketIsClosed_VerifyFactoryTicketAlreadyClosedIsReturned()
        {
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<ICloseTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Closed);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Close(It.IsAny<int>(), It.IsAny<int>());

            mockFactory.Verify(v => v.TicketAlreadyClosed(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyClosed method.");
        }

        //[Test]
        //public async Task Close_VerifyThatSingleAsyncForGetUserByIdIsCalled()
        //{
        //    var userId = _fixture.Create<int>();
        //    var mockContext = new Mock<IContextRepository<ITicketContext>>();

        //    mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

        //    var service = CreateService(mockContext: mockContext);

        //    await service.Close(It.IsAny<int>(), userId);

        //    mockContext.Verify(v => v.SingleAsync(It.Is<GetUserById>(t => t._userId == userId)), Times.Once, "Should call the context's SingleAsync method exactly once for GetTicketById.");
        //}

        [Test]
        public async Task Close_WhenUserRecordIsNull_VerifyFactoryUserNotFoundIsReturned()
        {
            var userId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<ICloseTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Close(It.IsAny<int>(), userId);

            mockFactory.Verify(v => v.UserNotFound(It.IsAny<int>(), userId), Times.Once, "Should return the factory's UserNotFound method.");
        }

        [Test]
        public async Task Close_BeforeTicketIsClosed_VerifyBeforeTicketClosedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketClosedWorkflow>())).ReturnsAsync(new BeforeTicketClosedWorkflow(ticketId, userId));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.Close(ticketId, userId);

            mockWorkflowService.Verify(v => v.Process(It.Is<BeforeTicketClosedWorkflow>(w => w.TicketId == ticketId && w.UserId == userId)), Times.Once, "Should call the workflow service's Process method for BeforeTicketClosedWorkflow.");
        }

        [Test]
        public async Task Close_WhenBeforeTicketClosedWorkflowIsNotSuccessful_VerifyFactoryWorkflowFailedIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var mockTicket = new Mock<Ticket>();
            var mockBeforeTicketClosedWorkflow = new Mock<BeforeTicketClosedWorkflow>(It.IsAny<int>(), It.IsAny<int>());
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<ICloseTicketResultFactory>();

            mockBeforeTicketClosedWorkflow.SetupGet(s => s.Result).Returns(WorkflowResult.Failed);
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketClosedWorkflow>())).ReturnsAsync(mockBeforeTicketClosedWorkflow.Object);

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.Close(ticketId, userId);

            mockFactory.Verify(v => v.WorkflowFailed(ticketId, userId, mockBeforeTicketClosedWorkflow.Object), Times.Once, "Should return the factory's WorkflowFailed method.");
        }

        [Test]
        public async Task Close_WhenTicketCanBeClosed_VerifyTicketCloseMethodIsCalled()
        {
            var userId = _fixture.Create<int>();
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketClosedWorkflow>())).ReturnsAsync(new BeforeTicketClosedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.Close(It.IsAny<int>(), userId);

            mockTicket.Verify(v => v.Close(userId), Times.Once, "Should call the ticket's Close method.");
        }

        [Test]
        public async Task Close_WhenTicketIsClosed_VerifySaveAsyncIsCalled()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketClosedWorkflow>())).ReturnsAsync(new BeforeTicketClosedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.Close(It.IsAny<int>(), It.IsAny<int>());

            mockContext.Verify(v => v.SaveAsync(), Times.Once, "Should call the context's SaveAsync.");
        }

        [Test]
        public async Task Close_WhenTicketIsClosed_VerifyTickeClosedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            var ticket = new Ticket();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketClosedWorkflow>())).ReturnsAsync(new BeforeTicketClosedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.Close(ticketId, userId);

            mockWorkflowService.Verify(v => v.Process(It.Is<TicketClosedWorkflow>(w => w.TicketId == ticketId && w.UserId == userId)), Times.Once, "Should process a new TicketClosedWorkflow.");
        }

        [Test]
        public async Task Close_WhenTicketIsClosed_VerifyTickeClosedNotificationIsQueued()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockNotificationService = new Mock<INotificationService>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            var ticket = new Ticket();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketClosedWorkflow>())).ReturnsAsync(new BeforeTicketClosedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockContext: mockContext,
                mockNotificationService: mockNotificationService,
                mockWorkflowService: mockWorkflowService);

            await service.Close(ticketId, userId);

            mockNotificationService.Verify(v => v.Queue(It.Is<TicketClosedNotification>(n => n.TicketId == ticketId && n.UserId == userId)), Times.Once, "Should queue a new TicketClosedNotification.");
        }

        [Test]
        public async Task Close_WhenTicketIsClosed_VerifyFactoryClosedIsReturned()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<ICloseTicketResultFactory>();

            var ticket = new Ticket();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketClosedWorkflow>())).ReturnsAsync(new BeforeTicketClosedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.Close(It.IsAny<int>(), It.IsAny<int>());

            mockFactory.Verify(v => v.Closed(ticket), Times.Once, "Should return the factory's Closed method.");
        }

        private CloseTicketService CreateService(
            IMock<IContextRepository<ITicketContext>> mockContext = null,
            IMock<INotificationService> mockNotificationService = null,
            IMock<IWorkflowService> mockWorkflowService = null,
            IMock<ICloseTicketResultFactory> mockFactory = null)
        {
            mockContext ??= new Mock<IContextRepository<ITicketContext>>();
            mockNotificationService ??= new Mock<INotificationService>();
            mockWorkflowService ??= new Mock<IWorkflowService>();
            mockFactory ??= new Mock<ICloseTicketResultFactory>();

            return new CloseTicketService(
                mockContext.Object,
                mockNotificationService.Object,
                mockWorkflowService.Object,
                mockFactory.Object);
        }
    }
}