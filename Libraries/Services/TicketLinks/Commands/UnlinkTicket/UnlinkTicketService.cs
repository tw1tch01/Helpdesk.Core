using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.DomainModels.TicketLinks;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.TicketLinks.Events.UnlinkTicket;
using Helpdesk.Services.TicketLinks.Factories.UnlinkTickets;
using Helpdesk.Services.TicketLinks.Results;
using Helpdesk.Services.TicketLinks.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Commands.UnlinkTicket
{
    public class UnlinkTicketService : IUnlinkTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;
        private readonly IUnlinkTicketsResultFactory _factory;

        public UnlinkTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService,
            IUnlinkTicketsResultFactory factory)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
            _factory = factory;
        }

        public virtual async Task<UnlinkTicketsResult> Unlink(RemoveTicketsLink removeLink)
        {
            if (removeLink.IsSelfUnlink()) throw new ArgumentException("Cannot unlink a ticket from itself.");

            var ticketLinkSpec = new GetLinkedTicketsById(removeLink.FromTicketId, removeLink.ToTicketId)
                               | new GetLinkedTicketsById(removeLink.ToTicketId, removeLink.FromTicketId);

            var ticketLink = await _repository.SingleAsync(ticketLinkSpec);

            if (ticketLink == null) return _factory.TicketsNotLinked(removeLink.FromTicketId, removeLink.ToTicketId);

            var beforeWorkflow = await _workflowService.Process(new BeforeTicketsUnlinkedWorkflow(removeLink.FromTicketId, removeLink.ToTicketId));
            if (beforeWorkflow.Result != WorkflowResult.Succeeded) return _factory.WorkflowFailed(removeLink.FromTicketId, removeLink.ToTicketId, beforeWorkflow);

            _repository.Remove(ticketLink);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketsUnlinkedWorkflow(removeLink.FromTicketId, removeLink.ToTicketId));
            var notification = _notificationService.Queue(new TicketsUnlinkedNotification(removeLink.FromTicketId, removeLink.ToTicketId));
            await Task.WhenAll(workflow, notification);

            return _factory.Unlinked(removeLink.FromTicketId, removeLink.ToTicketId);
        }
    }
}