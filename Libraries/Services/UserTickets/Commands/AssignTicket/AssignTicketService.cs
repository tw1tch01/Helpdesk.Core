using System.Threading.Tasks;
using Data.Repositories;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Notifications;
using Helpdesk.Services.Tickets.Commands.AssignTicket;
using Helpdesk.Services.Tickets.Results.Invalid;
using Helpdesk.Services.Tickets.Specifications;
using Helpdesk.Services.Users.Results.Invalid;
using Helpdesk.Services.Users.Specifications;
using Helpdesk.Services.UserTickets.Events.AssignTicket;
using Helpdesk.Services.UserTickets.Results.Invalid;
using Helpdesk.Services.UserTickets.Results.Valid;
using Helpdesk.Services.UserTickets.Specifications;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.UserTickets.Commands.AssignTicket
{
    public class AssignTicketService : IAssignTicketService
    {
        private readonly IContextRepository<ITicketContext> _repository;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowService _workflowService;

        public AssignTicketService(
            IContextRepository<ITicketContext> repository,
            INotificationService notificationService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _workflowService = workflowService;
        }

        public virtual async Task<ProcessResult> Assign(int ticketId, int userId)
        {
            var ticket = await _repository.SingleAsync(new GetTicketById(ticketId).AsNoTracking());

            if (ticket == null) return new TicketNotFoundResult(ticketId);

            switch (ticket.GetStatus())
            {
                case TicketStatus.Resolved:
                    return new TicketAlreadyResolvedResult(ticketId, ticket.ResolvedBy.Value);

                case TicketStatus.Closed:
                    return new TicketAlreadyClosedResult(ticketId, ticket.ClosedBy.Value);
            };

            if (await _repository.SingleAsync(new GetUserById(userId).AsNoTracking()) == null) return new UserNotFoundResult(userId);

            if (await _repository.GetAsync(new GetUserTicketById(ticketId, userId).AsNoTracking()) != null) return new UserAlreadyAssignedToTicketResult(ticketId, userId);

            await _workflowService.Process(new BeforeTicketAssignedWorkflow(ticketId, userId));

            var userTicket = new UserTicket(ticketId, userId);
            await _repository.AddAsync(userTicket);
            await _repository.SaveAsync();

            var workflow = _workflowService.Process(new TicketAssignedWorkflow(ticketId, userId));
            var notification = _notificationService.Queue(new TicketAssignedNotification(ticketId, userId));
            await Task.WhenAll(workflow, notification);

            return new UserAssignedToTicketResult(ticketId, userId);
        }
    }
}