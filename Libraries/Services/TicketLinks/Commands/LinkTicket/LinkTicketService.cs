using System;
using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.TicketLinks.Events.LinkTicket;
using Helpdesk.Services.TicketLinks.Results.Invalid;
using Helpdesk.Services.TicketLinks.Results.Valid;
using Helpdesk.Services.TicketLinks.Specifications;
using Helpdesk.Services.Tickets.Results.Invalid;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.TicketLinks.Commands.LinkTicket
{
    public class LinkTicketService : ILinkTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;

        public LinkTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
        }

        public virtual async Task<ProcessResult> Link(int fromTicketId, int toTicketId, TicketLinkType linkType)
        {
            if (fromTicketId == toTicketId) throw new ArgumentException("Cannot link a ticket to itself.");

            if (_repository.SingleAsync(new GetTicketById(fromTicketId).AsNoTracking()) == null) return new TicketNotFoundResult(fromTicketId);

            if (_repository.SingleAsync(new GetTicketById(toTicketId).AsNoTracking()) == null) return new TicketNotFoundResult(toTicketId);

            var fromTicket = await _repository.GetAsync(new GetTicketLinkById(fromTicketId, toTicketId).AsNoTracking());

            if (fromTicket != null) return new TicketsAlreadyLinkedResult(fromTicketId, toTicketId, fromTicket.LinkType);

            var toTicket = await _repository.GetAsync(new GetTicketLinkById(toTicketId, fromTicketId).AsNoTracking());

            if (toTicket != null) return new TicketsAlreadyLinkedResult(toTicketId, fromTicketId, toTicket.LinkType);

            await _workflowService.Process(new BeforeTicketsLinkedWorkflow(fromTicketId, toTicketId, linkType));

            var ticketLink = new TicketLink(fromTicketId, toTicketId);
            await _repository.AddAsync(ticketLink);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketsLinkedWorkflow(fromTicketId, toTicketId, linkType));
            var notification = _notificationService.Queue(new TicketsLinkedNotification(fromTicketId, toTicketId, linkType));
            await Task.WhenAll(workflow, notification);

            return new TicketsLinkedResult(fromTicketId, toTicketId, linkType);
        }
    }
}