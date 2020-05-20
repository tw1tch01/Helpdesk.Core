using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Commands.ResolveTicket;
using Helpdesk.Services.Tickets.Events.ResolveTicket;
using Helpdesk.Services.Tickets.Factories.ResolveTicket;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class ResolveTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task Resolve_VerifyThatSingleAsyncForGetTicketByIdIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockContext: mockContext);

            await service.Resolve(ticketId, It.IsAny<int>());

            mockContext.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t._ticketId == ticketId)), Times.Once, "Should call the context's SingleAsync method exactly once for GetTicketById.");
        }

        [Test]
        public async Task Resolve_WhenTicketRecordIsNull_VerifyFactoryTicketNotFoundIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IResolveTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Resolve(ticketId, It.IsAny<int>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task Resolve_WhenTicketIsResolved_VerifyFactoryTicketAlreadyResolvedIsReturned()
        {
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IResolveTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Resolved);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Resolve(It.IsAny<int>(), It.IsAny<int>());

            mockFactory.Verify(v => v.TicketAlreadyResolved(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyResolved method.");
        }

        [Test]
        public async Task Resolve_WhenTicketIsClosed_VerifyFactoryTicketAlreadyClosedIsReturned()
        {
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IResolveTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Closed);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Resolve(It.IsAny<int>(), It.IsAny<int>());

            mockFactory.Verify(v => v.TicketAlreadyClosed(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyClosed method.");
        }

        [Test]
        public async Task Resolve_VerifyThatSingleAsyncForGetUserByIdIsCalled()
        {
            var userId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(mockContext: mockContext);

            await service.Resolve(It.IsAny<int>(), userId);

            //mockContext.Verify(v => v.SingleAsync(It.Is<GetUserById>(t => t._userId == userId)), Times.Once, "Should call the context's SingleAsync method exactly once for GetTicketById.");
        }

        [Test]
        public async Task Resolve_WhenUserRecordIsNull_VerifyFactoryUserNotFoundIsReturned()
        {
            var userId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IResolveTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Resolve(It.IsAny<int>(), userId);

            mockFactory.Verify(v => v.UserNotFound(It.IsAny<int>(), userId), Times.Once, "Should return the factory's UserNotFound method.");
        }

        [Test]
        public async Task Resolve_BeforeTicketIsResolved_VerifyBeforeTicketResolvedWorkflowIsProcessed()
        {
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketResolvedWorkflow>())).ReturnsAsync(new BeforeTicketResolvedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.Resolve(It.IsAny<int>(), It.IsAny<int>());

            mockWorkflowService.Verify(v => v.Process(It.IsAny<BeforeTicketResolvedWorkflow>()), Times.Once, "Should call the workflow service's Process method for BeforeTicketResolvedWorkflow.");
        }

        [Test]
        public async Task Resolve_WhenBeforeTicketResolvedWorkflowIsNotSuccessful_VerifyFactoryWorkflowFailedIsReturned()
        {
            var mockTicket = new Mock<Ticket>();
            var mockBeforeTicketResolvedWorkflow = new Mock<BeforeTicketResolvedWorkflow>(It.IsAny<int>(), It.IsAny<int>());
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IResolveTicketResultFactory>();

            mockBeforeTicketResolvedWorkflow.SetupGet(s => s.Result).Returns(WorkflowResult.Failed);
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketResolvedWorkflow>())).ReturnsAsync(mockBeforeTicketResolvedWorkflow.Object);

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.Resolve(It.IsAny<int>(), It.IsAny<int>());

            mockFactory.Verify(v => v.WorkflowFailed(It.IsAny<int>(), It.IsAny<int>(), mockBeforeTicketResolvedWorkflow.Object), Times.Once, "Should return the factory's WorkflowFailed method.");
        }

        [Test]
        public async Task Resolve_WhenTicketCanBeResolved_VerifyTicketResolveMethodIsCalled()
        {
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketResolvedWorkflow>())).ReturnsAsync(new BeforeTicketResolvedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.Resolve(It.IsAny<int>(), It.IsAny<int>());

            mockTicket.Verify(v => v.Resolve(It.IsAny<int>()), Times.Once, "Should call the ticket's Resolve method.");
        }

        [Test]
        public async Task Resolve_WhenTicketIsResolved_VerifySaveAsyncIsCalled()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketResolvedWorkflow>())).ReturnsAsync(new BeforeTicketResolvedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.Resolve(It.IsAny<int>(), It.IsAny<int>());

            mockContext.Verify(v => v.SaveAsync(), Times.Once, "Should call the context's SaveAsync.");
        }

        [Test]
        public async Task Resolve_WhenTicketIsResolved_VerifyTicketResolvedWorkflowIsProcessed()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            var ticket = new Ticket();
            //var user = new User();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync();
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketResolvedWorkflow>())).ReturnsAsync(new BeforeTicketResolvedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService);

            await service.Resolve(It.IsAny<int>(), It.IsAny<int>());

            //mockWorkflowService.Verify(v => v.Process(It.Is<TicketResolvedWorkflow>(a => a.TicketId == ticket.TicketId && a.UserId == user.UserId)), Times.Once, "Should process a new TicketResolvedWorkflow.");
        }

        [Test]
        public async Task Resolve_WhenTicketIsResolved_VerifyTicketResolveNotificationIsQueued()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockNotificationService = new Mock<INotificationService>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            var ticket = new Ticket();
            //var user = new User();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(user);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketResolvedWorkflow>())).ReturnsAsync(new BeforeTicketResolvedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockContext: mockContext,
                mockNotificationService: mockNotificationService,
                mockWorkflowService: mockWorkflowService);

            await service.Resolve(It.IsAny<int>(), It.IsAny<int>());

            //mockNotificationService.Verify(v => v.Queue(It.Is<TicketResolvedNotification>(a => a.TicketId == ticket.TicketId && a.UserId == user.UserId)), Times.Once, "Should queue a new TicketResolvedNotification.");
        }

        [Test]
        public async Task Resolve_WhenTicketIsResolved_VerifyFactoryResolvedIsReturned()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IResolveTicketResultFactory>();

            var ticket = new Ticket();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            //mockContext.Setup(s => s.SingleAsync(It.IsAny<GetUserById>())).ReturnsAsync(new User());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketResolvedWorkflow>())).ReturnsAsync(new BeforeTicketResolvedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockContext: mockContext,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.Resolve(It.IsAny<int>(), It.IsAny<int>());

            mockFactory.Verify(v => v.Resolved(ticket), Times.Once, "Should return the factory's Resolved method.");
        }

        private ResolveTicketService CreateService(
            IMock<IContextRepository<ITicketContext>> mockContext = null,
            IMock<INotificationService> mockNotificationService = null,
            IMock<IWorkflowService> mockWorkflowService = null,
            IMock<IResolveTicketResultFactory> mockFactory = null)
        {
            mockContext ??= new Mock<IContextRepository<ITicketContext>>();
            mockNotificationService ??= new Mock<INotificationService>();
            mockWorkflowService ??= new Mock<IWorkflowService>();
            mockFactory ??= new Mock<IResolveTicketResultFactory>();

            return new ResolveTicketService(
                mockContext.Object,
                mockNotificationService.Object,
                mockWorkflowService.Object,
                mockFactory.Object);
        }
    }
}