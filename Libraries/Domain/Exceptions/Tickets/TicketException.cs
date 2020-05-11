using System;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Domain.Exceptions.Tickets
{
    public abstract class TicketException : Exception
    {
        protected const string _ticketIdKey = nameof(Ticket.TicketId);

        protected TicketException(int ticketId, string message)
                    : base(message)
        {
            Data[_ticketIdKey] = ticketId;
        }

        protected TicketException(int ticketId, string message, Exception innerException)
            : base(message, innerException)
        {
            Data[_ticketIdKey] = ticketId;
        }
    }
}