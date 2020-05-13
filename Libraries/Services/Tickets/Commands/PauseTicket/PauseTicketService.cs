using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.PauseTicket;
using Helpdesk.Services.Tickets.Events.ReopenTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Commands.PauseTicket
{
    public class PauseTicketService : IPauseTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;

        public PauseTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
        }

        public virtual async Task<PauseTicketResult> Pause(int ticketId)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return PauseTicketResult.TicketNotFound(ticketId);

            switch (ticket.GetStatus())
            {
                case TicketStatus.Resolved:
                    return PauseTicketResult.TicketAlreadyResolved(ticket);

                case TicketStatus.Closed:
                    return PauseTicketResult.TicketAlreadyClosed(ticket);

                case TicketStatus.OnHold:
                    return PauseTicketResult.TicketAlreadyPaused(ticket);
            }

            var beforeWorkflow = await _workflowService.Process(new BeforeTicketPausedWorkflow(ticketId));
            if (beforeWorkflow.Result != WorkflowResult.Succeeded) return PauseTicketResult.WorkflowFailed(ticketId, beforeWorkflow);

            ticket.Pause();
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketReopenedWorkflow(ticketId));
            var notification = _notificationService.Queue(new TicketPausedNotification(ticketId));
            await Task.WhenAll(workflow, notification);

            return PauseTicketResult.Paused(ticket);
        }
    }
}