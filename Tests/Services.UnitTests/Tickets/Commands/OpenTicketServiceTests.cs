using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Commands.OpenTicket;
using Helpdesk.Services.Tickets.Events.OpenTicket;
using Helpdesk.Services.Tickets.Factories.OpenTicket;
using Helpdesk.Services.Workflows;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class OpenTicketServiceTests
    {
        [Test]
        public void Open_WhenNewTicketIsNull_ThrowsArgumentNullException()
        {
            var service = CreateService();
            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.Open(null));
        }

        [Test]
        public async Task Open_WhenNewTicketIsNotValid_VerifyFactoryValidationFailureIsReturned()
        {
            var newTicket = new NewTicket();
            var mockFactory = new Mock<IOpenTicketResultFactory>();
            var mockValidator = new Mock<IValidator<NewTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidator.Setup(m => m.ValidateAsync(newTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(false);

            var service = CreateService(
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            var result = await service.Open(newTicket);

            mockFactory.Verify(v => v.ValidationFailure(mockValidationResult.Object.Errors), Times.Once, "Should return the factory's ValidationFailure method.");
        }

        [Test]
        public async Task Open_VerifyAddAsyncIsCalled()
        {
            var newTicket = new NewTicket();
            var ticket = new Ticket();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<NewTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidator.Setup(m => m.ValidateAsync(newTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            mockMapper.Setup(m => m.Map<Ticket>(newTicket)).Returns(ticket);

            var service = CreateService(
                mockContext: mockContext,
                mockMapper: mockMapper,
                mockValidator: mockValidator);

            var result = await service.Open(newTicket);

            mockContext.Verify(v => v.AddAsync(ticket), Times.Once, "Should call the context's AddAsync method for the ticket.");
        }

        [Test]
        public async Task Open_VerifySaveAsyncIsCalled()
        {
            var newTicket = new NewTicket();
            var ticket = new Ticket();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<NewTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidator.Setup(m => m.ValidateAsync(newTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            //mockContext.Setup(m => m.SingleAsync(It.IsAny<GetClientById>())).ReturnsAsync(new Client());
            mockMapper.Setup(m => m.Map<Ticket>(newTicket)).Returns(ticket);

            var service = CreateService(
                mockContext: mockContext,
                mockMapper: mockMapper,
                mockValidator: mockValidator);

            var result = await service.Open(newTicket);

            mockContext.Verify(v => v.SaveAsync(), Times.Once, "Should call the context's SaveAsync.");
        }

        [Test]
        public async Task Open_WhenTicketIsAdded_VerifyFactoryOpenedIsReturned()
        {
            var newTicket = new NewTicket();
            var ticket = new Ticket();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockMapper = new Mock<IMapper>();
            var mockFactory = new Mock<IOpenTicketResultFactory>();
            var mockValidator = new Mock<IValidator<NewTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidator.Setup(m => m.ValidateAsync(newTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            //mockContext.Setup(m => m.SingleAsync(It.IsAny<GetClientById>())).ReturnsAsync(new Client());
            mockMapper.Setup(m => m.Map<Ticket>(newTicket)).Returns(ticket);

            var service = CreateService(
                mockContext: mockContext,
                mockMapper: mockMapper,
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            var result = await service.Open(newTicket);

            mockFactory.Verify(v => v.Opened(ticket), Times.Once, "Should call the factory's Opened method.");
        }

        [Test]
        public async Task Open_WhenTicketIsAdded_VerifyTickeOpenedWorkflowIsProcessed()
        {
            var newTicket = new NewTicket();
            var ticket = new Ticket();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockMapper = new Mock<IMapper>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockValidator = new Mock<IValidator<NewTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidator.Setup(m => m.ValidateAsync(newTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            //mockContext.Setup(m => m.SingleAsync(It.IsAny<GetClientById>())).ReturnsAsync(new Client());
            mockMapper.Setup(m => m.Map<Ticket>(newTicket)).Returns(ticket);

            var service = CreateService(
                mockContext: mockContext,
                mockMapper: mockMapper,
                mockWorkflowService: mockWorkflowService,
                mockValidator: mockValidator);

            var result = await service.Open(newTicket);

            mockWorkflowService.Verify(v => v.Process(It.Is<TicketOpenedWorkflow>(a => a.TicketId == ticket.TicketId)), Times.Once, "Should process a new TicketOpenedWorkflow.");
        }

        [Test]
        public async Task Open_WhenTicketIsAdded_VerifyTickeOpenedNotificationIsQueued()
        {
            var newTicket = new NewTicket();
            var ticket = new Ticket();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockMapper = new Mock<IMapper>();
            var mockNotificationService = new Mock<INotificationService>();
            var mockValidator = new Mock<IValidator<NewTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidator.Setup(m => m.ValidateAsync(newTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            //mockContext.Setup(m => m.SingleAsync(It.IsAny<GetClientById>())).ReturnsAsync(new Client());
            mockMapper.Setup(m => m.Map<Ticket>(newTicket)).Returns(ticket);

            var service = CreateService(
                mockContext: mockContext,
                mockMapper: mockMapper,
                mockNotificationService: mockNotificationService,
                mockValidator: mockValidator);

            var result = await service.Open(newTicket);

            mockNotificationService.Verify(v => v.Queue(It.Is<TicketOpenedNotification>(a => a.TicketId == ticket.TicketId)), Times.Once, "Should queue a new TicketOpenedNotification.");
        }

        private OpenTicketService CreateService(
            Mock<IContextRepository<ITicketContext>> mockContext = null,
            Mock<IMapper> mockMapper = null,
            Mock<INotificationService> mockNotificationService = null,
            Mock<IWorkflowService> mockWorkflowService = null,
            Mock<IOpenTicketResultFactory> mockFactory = null,
            Mock<IValidator<NewTicket>> mockValidator = null)
        {
            mockContext ??= new Mock<IContextRepository<ITicketContext>>();
            mockMapper ??= new Mock<IMapper>();
            mockNotificationService ??= new Mock<INotificationService>();
            mockWorkflowService ??= new Mock<IWorkflowService>();
            mockFactory ??= new Mock<IOpenTicketResultFactory>();
            mockValidator ??= new Mock<IValidator<NewTicket>>();

            return new OpenTicketService(
                mockContext.Object,
                mockMapper.Object,
                mockNotificationService.Object,
                mockWorkflowService.Object,
                mockFactory.Object,
                mockValidator.Object);
        }
    }
}