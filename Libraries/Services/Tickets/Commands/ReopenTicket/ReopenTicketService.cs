using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.ReopenTicket;
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

        public ReopenTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
        }

        public virtual async Task<ReopenTicketResult> Reopen(int ticketId)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return ReopenTicketResult.TicketNotFound(ticketId);

            var beforeWorkflow = await _workflowService.Process(new BeforeTicketReopenedWorkflow(ticketId));
            if (beforeWorkflow.Result != WorkflowResult.Succeeded) return ReopenTicketResult.WorkflowFailed(ticketId, beforeWorkflow);

            ticket.Reopen();
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketReopenedWorkflow(ticketId));
            var notification = _notificationService.Queue(new TicketReopenedNotification(ticketId));
            await Task.WhenAll(workflow, notification);

            return ReopenTicketResult.Reopened(ticketId);
        }
    }
}