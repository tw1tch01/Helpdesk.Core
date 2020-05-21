using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.DomainModels.Tickets.Validation;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.UpdateTicket;
using Helpdesk.Services.Tickets.Factories.UpdateTicket;
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
        private readonly IUpdateTicketResultFactory _factory;
        private readonly IValidator<EditTicket> _validator;

        public UpdateTicketService(
            IContextRepository<ITicketContext> repository,
            IMapper mapper,
            INotificationService notificationService,
            IWorkflowService workflowService,
            IUpdateTicketResultFactory factory,
            IValidator<EditTicket> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _notificationService = notificationService;
            _workflowService = workflowService;
            _factory = factory;
            _validator = validator;
        }

        public virtual async Task<UpdateTicketResult> Update(int ticketId, EditTicket updateTicket)
        {
            if (updateTicket == null) throw new ArgumentNullException(nameof(updateTicket));

            var validationResult = await _validator.ValidateAsync(updateTicket);

            if (!validationResult.IsValid) return _factory.ValidationFailure(ticketId, validationResult.Errors);

            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            var changes = updateTicket.GetChanges(ticket);

            var beforeWorkflow = await _workflowService.Process(new BeforeTicketUpdatedWorkflow(ticketId, changes));
            if (beforeWorkflow.Result != WorkflowResult.Succeeded) return _factory.WorkflowFailed(ticketId, beforeWorkflow);

            _mapper.Map(updateTicket, ticket);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketUpdatedWorkflow(ticketId, changes));
            var notification = _notificationService.Queue(new TicketUpdatedNotification(ticketId, changes));
            await Task.WhenAll(workflow, notification);

            return _factory.Updated(ticket, changes);
        }

        #region Private Methods

        private ValidationResult ValidateDto(DomainModels.Tickets.EditTicket ticketDto)
        {
            var validator = new EditTicketValidator();
            var result = validator.Validate(ticketDto);
            return result;
        }

        #endregion Private Methods
    }
}