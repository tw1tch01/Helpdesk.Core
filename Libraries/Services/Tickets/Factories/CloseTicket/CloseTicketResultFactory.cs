using System;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.CloseTicket
{
    public class CloseTicketResultFactory : ICloseTicketResultFactory
    {
        public CloseTicketResult Closed(Ticket ticket)
        {
            return new CloseTicketResult(TicketCloseResult.Closed)
            {
                TicketId = ticket.TicketId,
                UserId = ticket.ClosedBy.Value,
                ClosedBy = ticket.ClosedBy.Value,
                ClosedOn = ticket.ClosedOn.Value
            };
        }

        public CloseTicketResult TicketAlreadyClosed(Ticket ticket)
        {
            return new CloseTicketResult(TicketCloseResult.TicketAlreadyClosed)
            {
                TicketId = ticket.TicketId,
                ClosedBy = ticket.ClosedBy.Value,
                ClosedOn = ticket.ClosedOn.Value
            };
        }

        public CloseTicketResult TicketAlreadyResolved(Ticket ticket)
        {
            return new CloseTicketResult(TicketCloseResult.TicketAlreadyResolved)
            {
                TicketId = ticket.TicketId,
                ResolvedBy = ticket.ResolvedBy.Value,
                ResolvedOn = ticket.ResolvedOn.Value
            };
        }

        public CloseTicketResult TicketNotFound(int ticketId)
        {
            return new CloseTicketResult(TicketCloseResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        public CloseTicketResult UserNotFound(int ticketId, Guid userId)
        {
            return new CloseTicketResult(TicketCloseResult.UserNotFound)
            {
                TicketId = ticketId,
                UserId = userId
            };
        }

        public CloseTicketResult WorkflowFailed(int ticketId, Guid userId, IWorkflowProcess workflow)
        {
            return new CloseTicketResult(TicketCloseResult.WorkflowFailed)
            {
                TicketId = ticketId,
                UserId = userId,
                Workflow = workflow
            };
        }
    }
}