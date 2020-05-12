using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.CloseTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Users.Specifications;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Commands.CloseTicket
{
    public class CloseTicketService : ICloseTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;

        public CloseTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
        }

        public virtual async Task<CloseTicketResult> Close(int ticketId, int userId)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return CloseTicketResult.TicketNotFound(ticketId);

            switch (ticket.GetStatus())
            {
                case TicketStatus.Resolved:
                    return CloseTicketResult.TicketAlreadyResolved(ticket);

                case TicketStatus.Closed:
                    return CloseTicketResult.TicketAlreadyClosed(ticket);
            }

            var user = await _repository.SingleAsync(new GetUserById(userId));

            if (user == null) return CloseTicketResult.UserNotFound(ticketId, userId);

            await _workflowService.Process(new BeforeTicketClosedWorkflow(ticketId, userId));

            ticket.Close(userId);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketClosedWorkflow(ticketId, userId));
            var notification = _notificationService.Queue(new TicketClosedNotification(ticketId, userId));
            await Task.WhenAll(workflow, notification);

            return CloseTicketResult.Closed(ticket);
        }
    }
}