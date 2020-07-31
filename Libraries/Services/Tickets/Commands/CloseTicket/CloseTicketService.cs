using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.CloseTicket;
using Helpdesk.Services.Tickets.Factories.CloseTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Commands.CloseTicket
{
    public class CloseTicketService : ICloseTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;
        private readonly ICloseTicketResultFactory _factory;

        public CloseTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService,
            ICloseTicketResultFactory factory)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
            _factory = factory;
        }

        public virtual async Task<CloseTicketResult> Close(int ticketId, Guid userGuid)
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

            var beforeWorkflow = await _workflowService.Process(new BeforeTicketClosedWorkflow(ticketId, userGuid));
            if (beforeWorkflow.Result != WorkflowResult.Succeeded) return _factory.WorkflowFailed(ticketId, userGuid, beforeWorkflow);

            ticket.Close(userGuid);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketClosedWorkflow(ticketId, userGuid));
            var notification = _notificationService.Queue(new TicketClosedNotification(ticketId, userGuid));
            await Task.WhenAll(workflow, notification);

            return _factory.Closed(ticket);
        }
    }
}