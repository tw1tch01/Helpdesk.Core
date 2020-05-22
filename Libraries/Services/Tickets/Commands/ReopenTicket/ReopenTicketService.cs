using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.ReopenTicket;
using Helpdesk.Services.Tickets.Factories.ReopenTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Commands.ReopenTicket
{
    public class ReopenTicketService : IReopenTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;
        private readonly IReopenTicketResultFactory _factory;

        public ReopenTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService,
            IReopenTicketResultFactory factory)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
            _factory = factory;
        }

        public virtual async Task<ReopenTicketResult> Reopen(int ticketId, Guid userGuid)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            var beforeWorkflow = await _workflowService.Process(new BeforeTicketReopenedWorkflow(ticketId, userGuid));
            if (beforeWorkflow.Result != WorkflowResult.Succeeded) return _factory.WorkflowFailed(ticketId, userGuid, beforeWorkflow);

            ticket.Reopen();
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketReopenedWorkflow(ticketId, userGuid));
            var notification = _notificationService.Queue(new TicketReopenedNotification(ticketId, userGuid));
            await Task.WhenAll(workflow, notification);

            return _factory.Reopened(ticket, userGuid);
        }
    }
}