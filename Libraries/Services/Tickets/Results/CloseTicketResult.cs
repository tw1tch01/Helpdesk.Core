using System;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Results
{
    public class CloseTicketResult
    {
        public CloseTicketResult(TicketCloseResult result)
        {
            Result = result;
        }

        public TicketCloseResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset? ResolvedOn { get; set; }
        public int? ResolvedBy { get; set; }
        public DateTimeOffset? ClosedOn { get; set; }
        public int? ClosedBy { get; set; }

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
                ResolvedBy = ticket.ResolvedBy,
                ResolvedOn = ticket.ResolvedOn
            };
        }

        internal static CloseTicketResult TicketAlreadyClosed(Ticket ticket)
        {
            return new CloseTicketResult(TicketCloseResult.TicketAlreadyClosed)
            {
                TicketId = ticket.TicketId,
                ClosedBy = ticket.ClosedBy,
                ClosedOn = ticket.ClosedOn
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
                ClosedBy = ticket.ClosedBy,
                ClosedOn = ticket.ClosedOn
            };
        }

        private string GetMessage()
        {
            return Result switch
            {
                TicketCloseResult.Closed => TicketResultMessages.Closed,
                TicketCloseResult.TicketNotFound => TicketResultMessages.TicketNotFound,
                TicketCloseResult.TicketAlreadyResolved => TicketResultMessages.TicketAlreadyResolved,
                TicketCloseResult.TicketAlreadyClosed => TicketResultMessages.TicketAlreadyClosed,
                TicketCloseResult.UserNotFound => TicketResultMessages.UserNotFound,
                _ => Result.ToString()
            };
        }
    }
}