using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Commands.DeleteTicket;
using Helpdesk.Services.Tickets.Events.DeleteTicket;
using Helpdesk.Services.Tickets.Factories.DeleteTicket;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class DeleteTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task Delete_VerifySingleAsyncForGetTicketByIdIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockRepository: mockRepository);

            await service.Delete(ticketId, It.IsAny<int>());

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t._ticketId == ticketId)), Times.Once, "Should call the repository's SingleAsync exactly once for GetTicketById.");
        }

        [Test]
        public async Task Delete_WhenTicketRecordIsNull_VerifyFactoryTicketNotFoundIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IDeleteTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Delete(ticketId, It.IsAny<int>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task Delete_BeforeTicketIsDeleted_VerifyBeforeTicketDeletedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketDeletedWorkflow>())).ReturnsAsync(new BeforeTicketDeletedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Delete(ticketId, userId);

            mockWorkflowService.Verify(v => v.Process(It.Is<BeforeTicketDeletedWorkflow>(w => w.TicketId == ticketId && w.UserId == userId)), Times.Once, "Should call the workflow services's Process method for BeforeTicketDeletedWorkflow.");
        }

        [Test]
        public async Task Delete_WhenBeforeTicketDeletedWorkflowIsNotSuccessful_VerifyFactoryWorkflowFailedIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var mockWorkflow = new Mock<BeforeTicketDeletedWorkflow>(It.IsAny<int>(), It.IsAny<int>());
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IDeleteTicketResultFactory>();

            mockWorkflow.Setup(a => a.Result).Returns(WorkflowResult.Failed);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketDeletedWorkflow>())).ReturnsAsync(mockWorkflow.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.Delete(ticketId, userId);

            mockFactory.Verify(v => v.WorkflowFailed(ticketId, userId, mockWorkflow.Object), Times.Once, "Should return the factory's WorkflowFailed method.");
        }

        [Test]
        public async Task Delete_VerifyRemoveForTicketIsCalled()
        {
            var ticket = new Ticket();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketDeletedWorkflow>())).ReturnsAsync(new BeforeTicketDeletedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Delete(It.IsAny<int>(), It.IsAny<int>());

            mockRepository.Verify(v => v.Remove(ticket), Times.Once, "Should call the repository's Remove method.");
        }

        [Test]
        public async Task Delete_VerifySaveAsyncIsCalled()
        {
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketDeletedWorkflow>())).ReturnsAsync(new BeforeTicketDeletedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Delete(It.IsAny<int>(), It.IsAny<int>());

            mockRepository.Verify(v => v.SaveAsync(), Times.Once, "Should call the repository's SaveAsync method.");
        }

        [Test]
        public async Task Delete_WhenTicketIsDeleted_VerifyTicketDeletedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketDeletedWorkflow>())).ReturnsAsync(new BeforeTicketDeletedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService);

            await service.Delete(ticketId, userId);

            mockWorkflowService.Verify(v => v.Process(It.Is<TicketDeletedWorkflow>(t => t.TicketId == ticketId && t.UserId == userId)), Times.Once, "Should call the workflow service's Process method for TicketDeletedWorkflow.");
        }

        [Test]
        public async Task Delete_WhenTicketIsDeleted_VerifyTicketDeletedNotificationIsQueued()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockNotificationService = new Mock<INotificationService>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketDeletedWorkflow>())).ReturnsAsync(new BeforeTicketDeletedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockNotificationService: mockNotificationService,
                mockWorkflowService: mockWorkflowService);

            await service.Delete(ticketId, userId);

            mockNotificationService.Verify(v => v.Queue(It.Is<TicketDeletedNotification>(t => t.TicketId == ticketId && t.UserId == userId)), Times.Once, "Should call the notification service's Queue method for TicketDeletedNotification.");
        }

        [Test]
        public async Task Delete_WhenTicketIsDeleted_VerifyFactoryDeletedIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IDeleteTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketDeletedWorkflow>())).ReturnsAsync(new BeforeTicketDeletedWorkflow(It.IsAny<int>(), It.IsAny<int>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory);

            await service.Delete(ticketId, userId);

            mockFactory.Verify(v => v.Deleted(ticketId, userId), Times.Once, "Should return the factoy's Deleted method.");
        }

        private DeleteTicketService CreateService(
            Mock<IContextRepository<ITicketContext>> mockRepository = null,
            Mock<INotificationService> mockNotificationService = null,
            Mock<IWorkflowService> mockWorkflowService = null,
            Mock<IDeleteTicketResultFactory> mockFactory = null)
        {
            mockRepository ??= new Mock<IContextRepository<ITicketContext>>();
            mockNotificationService ??= new Mock<INotificationService>();
            mockWorkflowService ??= new Mock<IWorkflowService>();
            mockFactory ??= new Mock<IDeleteTicketResultFactory>();

            return new DeleteTicketService(
                mockRepository.Object,
                mockNotificationService.Object,
                mockWorkflowService.Object,
                mockFactory.Object);
        }
    }
}