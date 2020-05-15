using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using FluentValidation;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Clients.Specifications;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Projects.Specifications;
using Helpdesk.Services.Tickets.Events.OpenTicket;
using Helpdesk.Services.Tickets.Factories.OpenTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Commands.OpenTicket
{
    public class OpenTicketService : IOpenTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;
        private readonly IOpenTicketResultFactory _factory;
        private readonly IValidator<NewTicket> _validator;

        public OpenTicketService(
            IContextRepository<ITicketContext> repository,
            IMapper mapper,
            INotificationService notificationService,
            IWorkflowService workflowService,
            IOpenTicketResultFactory factory,
            IValidator<NewTicket> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _notificationService = notificationService;
            _workflowService = workflowService;
            _factory = factory;
            _validator = validator;
        }

        public virtual async Task<OpenTicketResult> Open(NewTicket newTicket)
        {
            if (newTicket == null) throw new ArgumentNullException(nameof(newTicket));

            var validationResult = await _validator.ValidateAsync(newTicket);

            if (!validationResult.IsValid) return _factory.ValidationFailure(validationResult.Errors);

            var client = await _repository.SingleAsync(new GetClientById(newTicket.ClientId).AsNoTracking());

            if (client == null) return _factory.ClientNotFound(newTicket.ClientId);

            if (newTicket.ProjectId.HasValue)
            {
                var project = await _repository.SingleAsync(new GetProjectById(newTicket.ProjectId.Value).AsNoTracking());

                if (project == null) return _factory.ProjectNotFound(newTicket.ProjectId.Value);

                if (client.OrganizationId != project.OrganizationId) return _factory.ProjectInaccessible(client.ClientId, project.ProjectId);
            }

            var ticket = _mapper.Map<Ticket>(newTicket);

            await _repository.AddAsync(ticket);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketOpenedWorkflow(ticket.TicketId));
            var notification = _notificationService.Queue(new TicketOpenedNotification(ticket.TicketId));
            await Task.WhenAll(workflow, notification);

            return _factory.Opened(ticket);
        }
    }
}