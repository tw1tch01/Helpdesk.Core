using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Data.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Common;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Commands.UpdateTicket;
using Helpdesk.Services.Tickets.Events.UpdateTicket;
using Helpdesk.Services.Tickets.Factories.UpdateTicket;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class UpdateTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void Open_WhenUpdateTicketDtoIsNull_ThrowsArgumentNullException()
        {
            var service = CreateService();
            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.Update(It.IsAny<int>(), null));
        }

        [Test]
        public async Task Open_WhenUpdateTicketDtoIsNotValid_VerifyFactoryValidationFailureIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var mockFactory = new Mock<IUpdateTicketResultFactory>();
            var mockValidator = new Mock<IValidator<EditTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidator.Setup(m => m.ValidateAsync(updateTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(false);

            var service = CreateService(
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            var result = await service.Update(ticketId, updateTicket);

            mockFactory.Verify(v => v.ValidationFailure(ticketId, mockValidationResult.Object.Errors), Times.Once, "Should return the factory's ValidationFailure method.");
        }

        [Test]
        public async Task Open_VerifySingleAsyncForGetTicketByIdIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockValidator = new Mock<IValidator<EditTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidator.Setup(m => m.ValidateAsync(updateTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);

            var service = CreateService(
                mockRepository: mockRepository,
                mockValidator: mockValidator);

            var result = await service.Update(ticketId, updateTicket);

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetTicketById>(c => c._ticketId == ticketId)), Times.Once, "Should call the context's SingleAsync method exactly once for GetTicketById.");
        }

        [Test]
        public async Task Open_WhenClientRecordIsNull_VerifyFactoryTicketNotFoundIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IUpdateTicketResultFactory>();
            var mockValidator = new Mock<IValidator<EditTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidator.Setup(m => m.ValidateAsync(updateTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            mockRepository.Setup(m => m.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            var result = await service.Update(ticketId, updateTicket);

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task Update_BeforeTicketIsUpdated_VerifyBeforeTicketUpdatedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var ticket = new Ticket();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IUpdateTicketResultFactory>();
            var mockValidator = new Mock<IValidator<EditTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidator.Setup(m => m.ValidateAsync(updateTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            mockRepository.Setup(m => m.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);

            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketUpdatedWorkflow>())).ReturnsAsync(new BeforeTicketUpdatedWorkflow(ticketId, It.IsAny<IReadOnlyDictionary<string, ValueChange>>()));

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockValidator: mockValidator);

            await service.Update(ticketId, updateTicket);

            mockWorkflowService.Verify(v => v.Process(It.Is<BeforeTicketUpdatedWorkflow>(w => w.TicketId == ticketId)), Times.Once, "Should call the workflow flow's Process method for BeforeTicketUpdatedWorkflow.");
        }

        [Test]
        public async Task Start_WhenBeforeTicketUpdatedWorkflowIsNotSuccessful_VerifyFactoryWorkflowFailedIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var mockWorkflow = new Mock<BeforeTicketUpdatedWorkflow>(It.IsAny<int>(), It.IsAny<IReadOnlyDictionary<string, ValueChange>>());
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockFactory = new Mock<IUpdateTicketResultFactory>();
            var mockValidator = new Mock<IValidator<EditTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockWorkflow.Setup(a => a.Result).Returns(WorkflowResult.Failed);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketUpdatedWorkflow>())).ReturnsAsync(mockWorkflow.Object);
            mockValidator.Setup(m => m.ValidateAsync(updateTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            await service.Update(ticketId, updateTicket);

            mockFactory.Verify(v => v.WorkflowFailed(ticketId, mockWorkflow.Object), Times.Once, "Should return the factory's WorkflowFailed method.");
        }

        [Test]
        public async Task Open_VerifySaveAsyncIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var ticket = new Ticket();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<EditTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketUpdatedWorkflow>())).ReturnsAsync(new BeforeTicketUpdatedWorkflow(ticketId, It.IsAny<IReadOnlyDictionary<string, ValueChange>>()));
            mockValidator.Setup(m => m.ValidateAsync(updateTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            mockRepository.Setup(m => m.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockMapper.Setup(m => m.Map<Ticket>(updateTicket)).Returns(ticket);

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockMapper: mockMapper,
                mockValidator: mockValidator);

            var result = await service.Update(ticketId, updateTicket);

            mockRepository.Verify(v => v.SaveAsync(), Times.Once, "Should call the context's SaveAsync.");
        }

        [Test]
        public async Task Open_WhenTicketIsAdded_VerifyFactoryUpdatedIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var ticket = new Ticket();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockMapper = new Mock<IMapper>();
            var mockFactory = new Mock<IUpdateTicketResultFactory>();
            var mockValidator = new Mock<IValidator<EditTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketUpdatedWorkflow>())).ReturnsAsync(new BeforeTicketUpdatedWorkflow(ticketId, It.IsAny<IReadOnlyDictionary<string, ValueChange>>()));
            mockValidator.Setup(m => m.ValidateAsync(updateTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            mockRepository.Setup(m => m.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockMapper.Setup(m => m.Map<Ticket>(updateTicket)).Returns(ticket);

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockMapper: mockMapper,
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            var result = await service.Update(ticketId, updateTicket);

            mockFactory.Verify(v => v.Updated(ticket, It.IsAny<IReadOnlyDictionary<string, ValueChange>>()), Times.Once, "Should call the factory's Updated method.");
        }

        [Test]
        public async Task Open_WhenTicketIsAdded_VerifyTickeUpdatedWorkflowIsProcessed()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var ticket = new Ticket();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<EditTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketUpdatedWorkflow>())).ReturnsAsync(new BeforeTicketUpdatedWorkflow(ticketId, It.IsAny<IReadOnlyDictionary<string, ValueChange>>()));
            mockValidator.Setup(m => m.ValidateAsync(updateTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            mockRepository.Setup(m => m.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockMapper.Setup(m => m.Map<Ticket>(updateTicket)).Returns(ticket);

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockMapper: mockMapper,
                mockValidator: mockValidator);

            var result = await service.Update(ticketId, updateTicket);

            mockWorkflowService.Verify(v => v.Process(It.Is<TicketUpdatedWorkflow>(a => a.TicketId == ticketId)), Times.Once, "Should process a new TicketUpdatedWorkflow.");
        }

        [Test]
        public async Task Open_WhenTicketIsAdded_VerifyTickeUpdatedNotificationIsQueued()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var ticket = new Ticket();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockWorkflowService = new Mock<IWorkflowService>();
            var mockNotificationService = new Mock<INotificationService>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<EditTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockWorkflowService.Setup(s => s.Process(It.IsAny<BeforeTicketUpdatedWorkflow>())).ReturnsAsync(new BeforeTicketUpdatedWorkflow(ticketId, It.IsAny<IReadOnlyDictionary<string, ValueChange>>()));
            mockValidator.Setup(m => m.ValidateAsync(updateTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            mockRepository.Setup(m => m.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockMapper.Setup(m => m.Map<Ticket>(updateTicket)).Returns(ticket);

            var service = CreateService(
                mockRepository: mockRepository,
                mockWorkflowService: mockWorkflowService,
                mockMapper: mockMapper,
                mockNotificationService: mockNotificationService,
                mockValidator: mockValidator);

            var result = await service.Update(ticketId, updateTicket);

            mockNotificationService.Verify(v => v.Queue(It.Is<TicketUpdatedNotification>(a => a.TicketId == ticketId)), Times.Once, "Should queue a new TicketUpdatedNotification.");
        }

        private UpdateTicketService CreateService(
            Mock<IContextRepository<ITicketContext>> mockRepository = null,
            Mock<IMapper> mockMapper = null,
            Mock<INotificationService> mockNotificationService = null,
            Mock<IWorkflowService> mockWorkflowService = null,
            Mock<IUpdateTicketResultFactory> mockFactory = null,
            Mock<IValidator<EditTicket>> mockValidator = null)
        {
            mockRepository ??= new Mock<IContextRepository<ITicketContext>>();
            mockMapper ??= new Mock<IMapper>();
            mockNotificationService ??= new Mock<INotificationService>();
            mockWorkflowService ??= new Mock<IWorkflowService>();
            mockFactory ??= new Mock<IUpdateTicketResultFactory>();
            mockValidator ??= new Mock<IValidator<EditTicket>>();

            return new UpdateTicketService(
                mockRepository.Object,
                mockMapper.Object,
                mockNotificationService.Object,
                mockWorkflowService.Object,
                mockFactory.Object,
                mockValidator.Object);
        }
    }
}