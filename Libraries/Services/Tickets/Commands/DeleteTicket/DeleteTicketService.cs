using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.DeleteTicket;
using Helpdesk.Services.Tickets.Factories.DeleteTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Commands.DeleteTicket
{
    public class DeleteTicketService : IDeleteTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;
        private readonly IDeleteTicketResultFactory _factory;

        public DeleteTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService,
            IDeleteTicketResultFactory factory)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
            _factory = factory;
        }

        public virtual async Task<DeleteTicketResult> Delete(int ticketId, int userId)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return _factory.TicketNotFound(ticketId);

            var beforeWorkflow = await _workflowService.Process(new BeforeTicketDeletedWorkflow(ticketId, userId));
            if (beforeWorkflow.Result != WorkflowResult.Succeeded) return _factory.WorkflowFailed(ticketId, userId, beforeWorkflow);

            _repository.Remove(ticket);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketDeletedWorkflow(ticketId, userId));
            var notification = _notificationService.Queue(new TicketDeletedNotification(ticketId, userId));
            await Task.WhenAll(workflow, notification);

            return _factory.Deleted(ticketId, userId);
        }
    }
}