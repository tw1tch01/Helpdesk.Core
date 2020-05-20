using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.ResolveTicket;
using Helpdesk.Services.Tickets.Factories.ResolveTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Commands.ResolveTicket
{
    public class ResolveTicketService : IResolveTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;
        private readonly IResolveTicketResultFactory _factory;

        public ResolveTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService,
            IResolveTicketResultFactory factory)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
            _factory = factory;
        }

        public virtual async Task<ResolveTicketResult> Resolve(int ticketId, int userId)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            switch (ticket.GetStatus())
            {
                case TicketStatus.Resolved:
                    return _factory.TicketAlreadyResolved(ticket);

                case TicketStatus.Closed:
                    return _factory.TicketAlreadyClosed(ticket);
            }

            //var user = await _repository.SingleAsync(new GetUserById(userId));

            //if (user == null) return _factory.UserNotFound(ticketId, userId);

            var beforeWorkflow = await _workflowService.Process(new BeforeTicketResolvedWorkflow(ticketId, userId));
            if (beforeWorkflow.Result != WorkflowResult.Succeeded) return _factory.WorkflowFailed(ticketId, userId, beforeWorkflow);

            ticket.Resolve(userId);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketResolvedWorkflow(ticketId, userId));
            var notification = _notificationService.Queue(new TicketResolvedNotification(ticketId, userId));
            await Task.WhenAll(workflow, notification);

            return _factory.Resolved(ticket);
        }
    }
}