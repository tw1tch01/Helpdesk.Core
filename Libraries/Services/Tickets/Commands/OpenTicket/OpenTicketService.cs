using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using FluentValidation.Results;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.DomainModels.Tickets.Validation;
using Helpdesk.Services.Clients.Specifications;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Projects.Specifications;
using Helpdesk.Services.Tickets.Events.OpenTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
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

        public virtual async Task<OpenTicketResult> Open(OpenTicketDto ticketDto)
        {
            if (ticketDto == null) throw new ArgumentNullException(nameof(ticketDto));

            var validationResult = ValidateDto(ticketDto);

            if (!validationResult.IsValid) return new OpenTicketResult(validationResult.Errors);

            var client = await _repository.SingleAsync(new GetClientById(ticketDto.ClientId));

            if (client == null) return new OpenTicketResult(TicketOpenResult.ClientNotFound)
            {
                ClientId = client.ClientId
            };

            if (ticketDto.ProjectId.HasValue)
            {
                var project = await _repository.SingleAsync(new GetProjectById(ticketDto.ProjectId.Value));

                if (project == null) return new OpenTicketResult(TicketOpenResult.ProjectNotFound)
                {
                    ProjectId = ticketDto.ProjectId.Value
                };

                if (client.OrganizationId != project.OrganizationId) return new OpenTicketResult(TicketOpenResult.ProjectInaccessible)
                {
                    ClientId = client.ClientId,
                    ProjectId = project.ProjectId
                };
            }

            var ticket = _mapper.Map<Ticket>(ticketDto);

            await _repository.AddAsync(ticket);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketOpenedWorkflow(ticket.TicketId));
            var notification = _notificationService.Queue(new TicketOpenedNotification(ticket.TicketId));
            await Task.WhenAll(workflow, notification);

            return new OpenTicketResult(TicketOpenResult.Opened)
            {
                TicketId = ticket.TicketId,
                ClientId = ticket.ClientId,
                ProjectId = ticket.ProjectId
            };
        }

        private ValidationResult ValidateDto(OpenTicketDto ticketDto)
        {
            var validator = new OpenTicketValidator();
            var result = validator.Validate(ticketDto);
            return result;
        }
    }
}