using System;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Results
{
    public class CloseTicketResult : IProcessResult<TicketCloseResult>, IWorkflowResult
    {
        public CloseTicketResult(TicketCloseResult result)
        {
            Result = result;
        }

        public TicketCloseResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; private set; }
        public int UserId { get; private set; }
        public DateTimeOffset? ResolvedOn { get; private set; }
        public int? ResolvedBy { get; private set; }
        public DateTimeOffset? ClosedOn { get; private set; }
        public int? ClosedBy { get; private set; }
        public IWorkflowProcess Workflow { get; private set; }

        #region Methods

        internal static CloseTicketResult TicketNotFound(int ticketId)
        {
            return new CloseTicketResult(TicketCloseResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        internal static CloseTicketResult TicketAlreadyResolved(Ticket ticket)
        {
            return new CloseTicketResult(TicketCloseResult.TicketAlreadyResolved)
            {
                TicketId = ticket.TicketId,
                ResolvedBy = ticket.ResolvedBy.Value,
                ResolvedOn = ticket.ResolvedOn.Value
            };
        }

        internal static CloseTicketResult TicketAlreadyClosed(Ticket ticket)
        {
            return new CloseTicketResult(TicketCloseResult.TicketAlreadyClosed)
            {
                TicketId = ticket.TicketId,
                ClosedBy = ticket.ClosedBy.Value,
                ClosedOn = ticket.ClosedOn.Value
            };
        }

        internal static CloseTicketResult UserNotFound(int ticketId, int userId)
        {
            return new CloseTicketResult(TicketCloseResult.UserNotFound)
            {
                TicketId = ticketId,
                UserId = userId
            };
        }

        internal static CloseTicketResult Closed(Ticket ticket)
        {
            return new CloseTicketResult(TicketCloseResult.Closed)
            {
                TicketId = ticket.TicketId,
                ClosedBy = ticket.ClosedBy.Value,
                ClosedOn = ticket.ClosedOn.Value
            };
        }

        internal static CloseTicketResult WorkflowFailed(int ticketId, IWorkflowProcess workflow)
        {
            return new CloseTicketResult(TicketCloseResult.WorkflowFailed)
            {
                TicketId = ticketId,
                Workflow = workflow
            };
        }

        private string GetMessage() => Result switch
        {
            TicketCloseResult.Closed => ResultMessages.Closed,
            TicketCloseResult.TicketNotFound => ResultMessages.TicketNotFound,
            TicketCloseResult.TicketAlreadyResolved => ResultMessages.TicketAlreadyResolved,
            TicketCloseResult.TicketAlreadyClosed => ResultMessages.TicketAlreadyClosed,
            TicketCloseResult.UserNotFound => ResultMessages.UserNotFound,
            TicketCloseResult.WorkflowFailed => ResultMessages.WorkflowFailed,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}