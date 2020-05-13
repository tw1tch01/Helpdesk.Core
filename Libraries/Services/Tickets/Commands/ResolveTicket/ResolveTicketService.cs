using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.ResolveTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Users.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Commands.ResolveTicket
{
    public class ResolveTicketService : IResolveTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;

        public ResolveTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
        }

        public virtual async Task<ResolveTicketResult> Resolve(int ticketId, int userId)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return ResolveTicketResult.TicketNotFound(ticketId);

            switch (ticket.GetStatus())
            {
                case TicketStatus.Resolved:
                    return ResolveTicketResult.TicketAlreadyResolved(ticket);

                case TicketStatus.Closed:
                    return ResolveTicketResult.TicketAlreadyClosed(ticket);
            }

            var user = await _repository.SingleAsync(new GetUserById(userId));

            if (user == null) return ResolveTicketResult.UserNotFound(ticketId, userId);

            var beforeWorkflow = await _workflowService.Process(new BeforeTicketResolvedWorkflow(ticketId, userId));
            if (beforeWorkflow.Result != WorkflowResult.Succeeded) return ResolveTicketResult.WorkflowFailed(ticketId, beforeWorkflow);

            ticket.Resolve(userId);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketResolvedWorkflow(ticketId, userId));
            var notification = _notificationService.Queue(new TicketResolvedNotification(ticketId, userId));
            await Task.WhenAll(workflow, notification);

            return ResolveTicketResult.Resolved(ticket);
        }
    }
}