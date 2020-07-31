using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.TicketLinks;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.TicketLinks.Events.LinkTicket;
using Helpdesk.Services.TicketLinks.Factories.LinkTickets;
using Helpdesk.Services.TicketLinks.Results;
using Helpdesk.Services.TicketLinks.Specifications;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.TicketLinks.Commands.LinkTickets
{
    public class LinkTicketService : ILinkTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;
        private readonly ILinkTicketsResultFactory _factory;

        public LinkTicketService(
            IContextRepository<ITicketContext> repository,
            IMapper mapper,
            INotificationService notificationService,
            IWorkflowService workflowService,
            ILinkTicketsResultFactory factory)
        {
            _repository = repository;
            _mapper = mapper;
            _notificationService = notificationService;
            _workflowService = workflowService;
            _factory = factory;
        }

        public virtual async Task<LinkTicketsResult> Link(NewTicketsLink newLink)
        {
            if (newLink.IsSelfLink()) throw new ArgumentException("Cannot link a ticket to itself.");

            var existingTicketLinkSpec = new GetLinkedTicketsById(newLink.FromTicketId, newLink.ToTicketId)
                                       | new GetLinkedTicketsById(newLink.ToTicketId, newLink.FromTicketId);

            var existingTicketLink = await _repository.SingleAsync(existingTicketLinkSpec.AsNoTracking());

            if (existingTicketLink != null) return _factory.TicketsAlreadyLinked(existingTicketLink);

            var beforeWorkflow = await _workflowService.Process(new BeforeTicketsLinkedWorkflow(newLink.FromTicketId, newLink.ToTicketId, newLink.LinkType));
            if (beforeWorkflow.Result != WorkflowResult.Succeeded) return _factory.WorkflowFailed(newLink.FromTicketId, newLink.ToTicketId, newLink.LinkType, beforeWorkflow);

            var ticketLink = _mapper.Map<TicketLink>(newLink);

            await _repository.AddAsync(ticketLink);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketsLinkedWorkflow(newLink.FromTicketId, newLink.ToTicketId, newLink.LinkType));
            var notification = _notificationService.Queue(new TicketsLinkedNotification(newLink.FromTicketId, newLink.ToTicketId, newLink.LinkType));
            await Task.WhenAll(workflow, notification);

            return _factory.Linked(ticketLink);
        }
    }
}