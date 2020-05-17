using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.TicketLinks.Events.UnlinkTicket;
using Helpdesk.Services.TicketLinks.Results.Invalid;
using Helpdesk.Services.TicketLinks.Results.Valid;
using Helpdesk.Services.TicketLinks.Specifications;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Commands.UnlinkTicket
{
    public class UnlinkTicketService : IUnlinkTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;

        public UnlinkTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
        }

        public virtual async Task<ProcessResult> Unlink(int fromTicketId, int toTicketId)
        {
            if (fromTicketId == toTicketId) throw new ArgumentException("Cannot unlink a ticket from itself.");

            var ticketLink = await _repository.SingleAsync(new GetTicketLinkById(fromTicketId, toTicketId));

            if (ticketLink == null) await _repository.SingleAsync(new GetTicketLinkById(toTicketId, fromTicketId));

            if (ticketLink == null) return new TicketsNotLinkedResult(fromTicketId, toTicketId);

            await _workflowService.Process(new BeforeTicketsUnlinkedWorkflow(fromTicketId, toTicketId));

            _repository.Remove(ticketLink);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketsUnlinkedWorkflow(fromTicketId, toTicketId));
            var notification = _notificationService.Queue(new TicketsUnlinkedNotification(fromTicketId, toTicketId));
            await Task.WhenAll(workflow, notification);

            return new TicketsUnlinkedResult(fromTicketId, toTicketId);
        }
    }
}