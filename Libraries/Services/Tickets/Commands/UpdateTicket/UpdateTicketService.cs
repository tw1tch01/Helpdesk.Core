﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using FluentValidation.Results;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.DomainModels.Tickets.Validation;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.UpdateTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Commands.UpdateTicket
{
    public class UpdateTicketService : IUpdateTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;

        public UpdateTicketService(
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

        public virtual async Task<UpdateTicketResult> Update(int ticketId, DomainModels.Tickets.UpdateTicket ticketDto)
        {
            if (ticketDto == null) throw new ArgumentNullException(nameof(ticketDto));

            var validationResult = ValidateDto(ticketDto);

            if (!validationResult.IsValid) return UpdateTicketResult.ValidationFailure(ticketId, validationResult.Errors);

            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return UpdateTicketResult.TicketNotFound(ticketId);

            var changes = ticketDto.GetChanges(ticket);

            var beforeWorkflow = await _workflowService.Process(new BeforeTicketUpdateWorkflow(ticketId, changes));
            if (beforeWorkflow.Result != WorkflowResult.Succeeded) return UpdateTicketResult.WorkflowFailed(ticketId, beforeWorkflow);

            _mapper.Map(ticketDto, ticket);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketUpdatedWorkflow(ticketId, changes));
            var notification = _notificationService.Queue(new TicketUpdatedNotification(ticketId, changes));
            await Task.WhenAll(workflow, notification);

            return UpdateTicketResult.Updated(ticket, changes);
        }

        #region Private Methods

        private ValidationResult ValidateDto(DomainModels.Tickets.UpdateTicket ticketDto)
        {
            var validator = new UpdateTicketValidator();
            var result = validator.Validate(ticketDto);
            return result;
        }

        #endregion Private Methods
    }
}