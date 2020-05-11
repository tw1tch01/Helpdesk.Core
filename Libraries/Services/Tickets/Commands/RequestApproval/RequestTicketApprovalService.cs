using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.RequestApproval;
using Helpdesk.Services.Tickets.Results.Invalid;
using Helpdesk.Services.Tickets.Results.Valid;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Users.Results.Invalid;
using Helpdesk.Services.Users.Specifications;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Commands.RequestApproval
{
    public class RequestTicketApprovalService : IRequestTicketApprovalService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;

        public RequestTicketApprovalService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
        }

        public virtual async Task<ProcessResult> RequestApproval(int ticketId, int userId)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return new TicketNotFoundResult(ticketId);

            switch (ticket.GetStatus())
            {
                case TicketStatus.Resolved:
                    return new TicketAlreadyResolvedResult(ticketId, ticket.ResolvedBy.Value);

                case TicketStatus.Closed:
                    return new TicketAlreadyClosedResult(ticketId, ticket.ClosedBy.Value);
            }

            var user = await _repository.SingleAsync(new GetUserById(userId));

            if (user == null) return new UserNotFoundResult(userId);

            await _workflowService.Process(new BeforeTicketApprovalRequestProcess(ticketId, userId));

            ticket.RequestApproval(userId);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketApprovalRequestWorkflow(ticketId, userId));
            var notification = _notificationService.Queue(new TicketApprovalRequestNotification(ticketId, userId));
            await Task.WhenAll(workflow, notification);

            return new TicketApprovedResult(ticketId, userId);
        }
    }
}