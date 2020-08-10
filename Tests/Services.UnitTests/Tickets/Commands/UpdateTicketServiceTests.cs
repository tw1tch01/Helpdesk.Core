using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Data.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Helpdesk.Domain.Common;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Events;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Commands.UpdateTicket;
using Helpdesk.Services.Tickets.Factories.UpdateTicket;
using Helpdesk.Services.Tickets.Specifications;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class UpdateTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void Update_WhenUpdateTicketDtoIsNull_ThrowsArgumentNullException()
        {
            var service = CreateService();
            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.Update(It.IsAny<int>(), null));
        }

        [Test]
        public async Task Update_WhenUpdateTicketDtoIsNotValid_VerifyFactoryValidationFailureIsReturned()
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
        public async Task Update_VerifySingleAsyncForGetTicketByIdIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();
            var mockValidator = new Mock<IValidator<EditTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidator.Setup(m => m.ValidateAsync(updateTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);

            var service = CreateService(
                mockRepository: mockRepository,
                mockValidator: mockValidator);

            var result = await service.Update(ticketId, updateTicket);

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetTicketById>(c => c.TicketId == ticketId)), Times.Once, "Should call the context's SingleAsync method exactly once for GetTicketById.");
        }

        [Test]
        public async Task Update_WhenClientRecordIsNull_VerifyFactoryTicketNotFoundIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();
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
        public async Task Update_VerifySaveAsyncIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var ticket = new Ticket();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<EditTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());
            mockValidator.Setup(m => m.ValidateAsync(updateTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            mockRepository.Setup(m => m.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockMapper.Setup(m => m.Map<Ticket>(updateTicket)).Returns(ticket);

            var service = CreateService(
                mockRepository: mockRepository,
                mockMapper: mockMapper,
                mockValidator: mockValidator);

            var result = await service.Update(ticketId, updateTicket);

            mockRepository.Verify(v => v.SaveAsync(), Times.Once, "Should call the context's SaveAsync.");
        }

        [Test]
        public async Task Update_WhenTicketIsUpdated_VerifyEventIsRaised()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var ticket = new Ticket();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<EditTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();
            var mockEventService = new Mock<IEventService>();

            mockValidator.Setup(m => m.ValidateAsync(updateTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            mockRepository.Setup(m => m.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockMapper.Setup(m => m.Map<Ticket>(updateTicket)).Returns(ticket);

            var service = CreateService(
                mockRepository: mockRepository,
                mockMapper: mockMapper,
                mockValidator: mockValidator,
                mockEventService: mockEventService);

            var result = await service.Update(ticketId, updateTicket);

            mockEventService.Verify(v => v.Publish(It.IsAny<TicketUpdatedEvent>()), Times.Once, "Should publish a TicketUpdatedEvent.");
        }

        [Test]
        public async Task Update_WhenTicketIsUpdated_VerifyFactoryUpdatedIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var updateTicket = new EditTicket();
            var ticket = new Ticket();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();
            var mockMapper = new Mock<IMapper>();
            var mockFactory = new Mock<IUpdateTicketResultFactory>();
            var mockValidator = new Mock<IValidator<EditTicket>>();
            var mockValidationResult = new Mock<ValidationResult>();

            mockValidator.Setup(m => m.ValidateAsync(updateTicket, It.IsAny<CancellationToken>())).ReturnsAsync(mockValidationResult.Object);
            mockValidationResult.Setup(m => m.IsValid).Returns(true);
            mockRepository.Setup(m => m.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);
            mockMapper.Setup(m => m.Map<Ticket>(updateTicket)).Returns(ticket);

            var service = CreateService(
                mockRepository: mockRepository,
                mockMapper: mockMapper,
                mockFactory: mockFactory,
                mockValidator: mockValidator);

            var result = await service.Update(ticketId, updateTicket);

            mockFactory.Verify(v => v.Updated(ticket, It.IsAny<IReadOnlyDictionary<string, ValueChange>>()), Times.Once, "Should call the factory's Updated method.");
        }

        private UpdateTicketService CreateService(
            IMock<IEntityRepository<ITicketContext>> mockRepository = null,
            IMock<IMapper> mockMapper = null,
            IMock<IUpdateTicketResultFactory> mockFactory = null,
            IMock<IValidator<EditTicket>> mockValidator = null,
            IMock<IEventService> mockEventService = null)
        {
            mockRepository ??= new Mock<IEntityRepository<ITicketContext>>();
            mockMapper ??= new Mock<IMapper>();
            mockFactory ??= new Mock<IUpdateTicketResultFactory>();
            mockValidator ??= new Mock<IValidator<EditTicket>>();
            mockEventService ??= new Mock<IEventService>();

            return new UpdateTicketService(
                mockRepository.Object,
                mockMapper.Object,
                mockFactory.Object,
                mockValidator.Object,
                mockEventService.Object);
        }
    }
}