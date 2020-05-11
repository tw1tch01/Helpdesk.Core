using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.PauseTicket;
using Helpdesk.Services.Tickets.Events.ReopenTicket;
using Helpdesk.Services.Tickets.Results.Invalid;
using Helpdesk.Services.Tickets.Results.Valid;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;

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

        public virtual async Task<ProcessResult> Pause(int ticketId)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return new TicketNotFoundResult(ticketId);

            switch (ticket.GetStatus())
            {
                case TicketStatus.Resolved:
                    return new TicketAlreadyResolvedResult(ticketId, ticket.ResolvedBy.Value);

                case TicketStatus.Closed:
                    return new TicketAlreadyClosedResult(ticketId, ticket.ClosedBy.Value);

                case TicketStatus.OnHold:
                    return new TicketAlreadyOnHoldResult(ticketId);
            }

            await _workflowService.Process(new BeforeTicketPausedWorkflow(ticketId));

            ticket.Pause();
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketReopenedWorkflow(ticketId));
            var notification = _notificationService.Queue(new TicketPausedNotification(ticketId));
            await Task.WhenAll(workflow, notification);

            return new TicketPausedResult(ticketId);
        }
    }
}