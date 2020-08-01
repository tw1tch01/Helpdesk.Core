using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Factories.PauseTicket
{
    public class PauseTicketResultFactory : IPauseTicketResultFactory
    {
        public PauseTicketResult Paused(Ticket ticket)
        {
            return new PauseTicketResult(TicketPauseResult.Paused)
            {
                TicketId = ticket.TicketId,
                PausedOn = ticket.PausedOn.Value
            };
        }

        public PauseTicketResult TicketAlreadyClosed(Ticket ticket)
        {
            return new PauseTicketResult(TicketPauseResult.TicketAlreadyClosed)
            {
                TicketId = ticket.TicketId,
                ClosedBy = ticket.ClosedBy.Value,
                ClosedOn = ticket.ClosedOn.Value,
            };
        }

        public PauseTicketResult TicketAlreadyPaused(Ticket ticket)
        {
            return new PauseTicketResult(TicketPauseResult.TicketAlreadyPaused)
            {
                TicketId = ticket.TicketId,
                PausedOn = ticket.PausedOn.Value
            };
        }

        public PauseTicketResult TicketAlreadyResolved(Ticket ticket)
        {
            return new PauseTicketResult(TicketPauseResult.TicketAlreadyResolved)
            {
                TicketId = ticket.TicketId,
                ResolvedBy = ticket.ResolvedBy.Value,
                ResolvedOn = ticket.ResolvedOn.Value,
            };
        }

        public PauseTicketResult TicketNotFound(int ticketId)
        {
            return new PauseTicketResult(TicketPauseResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }
    }
}