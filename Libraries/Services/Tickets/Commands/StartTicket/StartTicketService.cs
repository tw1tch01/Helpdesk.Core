using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.ReopenTicket;
using Helpdesk.Services.Tickets.Events.StartTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Commands.StartTicket
{
    public class StartTicketService : IStartTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;

        public StartTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
        }

        public virtual async Task<StartTicketResult> Start(int ticketId)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return StartTicketResult.TicketNotFound(ticketId);

            switch (ticket.GetStatus())
            {
                case TicketStatus.Resolved:
                    return StartTicketResult.TicketAlreadyResolved(ticket);

                case TicketStatus.Closed:
                    return StartTicketResult.TicketAlreadyClosed(ticket);

                case TicketStatus.InProgress:
                    return StartTicketResult.TicketAlreadyStarted(ticket);
            }

            var beforeWorkflow = await _workflowService.Process(new BeforeTicketStartedWorkflow(ticketId));
            if (beforeWorkflow.Result != WorkflowResult.Succeeded) return StartTicketResult.WorkflowFailed(ticketId, beforeWorkflow);

            ticket.Start();
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketReopenedWorkflow(ticketId));
            var notification = _notificationService.Queue(new TicketStartedNotification(ticketId));
            await Task.WhenAll(workflow, notification);

            return StartTicketResult.Started(ticket);
        }
    }
}