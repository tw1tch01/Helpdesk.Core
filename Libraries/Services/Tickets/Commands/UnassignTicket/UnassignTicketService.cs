using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.UnassignTicket;
using Helpdesk.Services.Tickets.Factories.UnassignTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Commands.UnassignTicket
{
    public class UnassignTicketService : IUnassignTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;
        private readonly IUnassignTicketResultFactory _factory;

        public UnassignTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService,
            IUnassignTicketResultFactory factory)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
            _factory = factory;
        }

        public virtual async Task<UnassignTicketResult> UnassignUser(int ticketId, Guid userGuid)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            var beforeWorkflow = await _workflowService.Process(new BeforeTicketUnassignedWorkflow(ticketId, userGuid));
            if (beforeWorkflow.Result != WorkflowResult.Succeeded) return _factory.WorkflowFailed(ticketId, userGuid, beforeWorkflow);

            ticket.UnassignUser();
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketUnassignedWorkflow(ticketId, userGuid));
            var notification = _notificationService.Queue(new TicketUnassignedNotification(ticketId, userGuid));
            await Task.WhenAll(workflow, notification);

            return _factory.Unassigned(ticketId, userGuid);
        }
    }
}