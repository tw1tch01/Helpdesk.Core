using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.UserTickets.Events.UnassignTicket;
using Helpdesk.Services.UserTickets.Results.Invalid;
using Helpdesk.Services.UserTickets.Results.Valid;
using Helpdesk.Services.UserTickets.Specifications;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.UserTickets.Commands.UnassignTicket
{
    public class UnassignTicketService : IUnassignTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;

        public UnassignTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
        }

        public virtual async Task<ProcessResult> Unassign(int ticketId, int userId)
        {
            var userTicket = await _repository.SingleAsync(new GetUserTicketById(ticketId, userId));

            if (userTicket == null) return new UserNotAssignedToTicketResult(ticketId, userId);

            await _workflowService.Process(new BeforeTicketUnassignedWorkflow(ticketId, userId));

            _repository.Remove(userTicket);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketUnassignedWorkflow(ticketId, userId));
            var notification = _notificationService.Queue(new TicketUnassignedNotification(ticketId, userId));
            await Task.WhenAll(workflow, notification);

            return new UserUnassignedFromTicketResult(ticketId, userId);
        }
    }
}