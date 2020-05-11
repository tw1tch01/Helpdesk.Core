using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.DomainModels.Tickets.Validation;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.OpenTicket;
using Helpdesk.Services.Tickets.Results.Valid;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Commands.OpenTicket
{
    public class OpenTicketService : IOpenTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;

        public OpenTicketService(
            IContextRepository<ITicketContext> repository,
            IMapper mapper,
            INotificationService notificationService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _mapper = mapper;
            _notificationService = notificationService;
            _workflowService = workflowService;
        }

        public virtual async Task<ProcessResult> Open(OpenTicketDto ticketDto)
        {
            if (ticketDto == null) throw new ArgumentNullException(nameof(ticketDto));

            var validationResult = ValidateDto(ticketDto);

            if (!validationResult.IsValid) return new ValidationFailureResult(validationResult.Errors);

            var ticket = _mapper.Map<Ticket>(ticketDto);

            await _repository.AddAsync(ticket);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketOpenedWorkflow(ticket.TicketId));
            var notification = _notificationService.Queue(new TicketOpenedNotification(ticket.TicketId));
            await Task.WhenAll(workflow, notification);

            return new TicketOpenedResult(ticket.TicketId);
        }

        private FluentValidation.Results.ValidationResult ValidateDto(OpenTicketDto ticketDto)
        {
            var validator = new OpenTicketValidator();
            var result = validator.Validate(ticketDto);
            return result;
        }
    }
}