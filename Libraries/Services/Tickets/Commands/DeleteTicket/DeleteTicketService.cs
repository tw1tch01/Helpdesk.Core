using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Services.Common;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Events.DeleteTicket;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Commands.DeleteTicket
{
    public class DeleteTicketService : IDeleteTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;

        public DeleteTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
        }

        public virtual async Task<DeleteTicketResult> Delete(int ticketId)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId));

            if (ticket == null) return DeleteTicketResult.TicketNotFound(ticketId);

            await _workflowService.Process(new BeforeTicketDeletedWorkflow(ticketId));

            _repository.Remove(ticket);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketDeletedWorkflow(ticketId));
            var notification = _notificationService.Queue(new TicketDeletedNotification(ticketId));
            await Task.WhenAll(workflow, notification);

            return DeleteTicketResult.Deleted(ticketId);
        }
    }
}