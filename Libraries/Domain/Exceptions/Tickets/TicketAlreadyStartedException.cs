using System;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Domain.Exceptions.Tickets
{
    public class TicketAlreadyStartedException : TicketException
    {
        private const string _message = "Ticket was already started on {0}.";
        private const string _startedOnKey = nameof(Ticket.StartedOn);

        public TicketAlreadyStartedException(int ticketId, DateTime startedOn)
            : base(ticketId, GetMessage(startedOn))
        {
            Data[_startedOnKey] = startedOn;
        }

        private static string GetMessage(DateTime startedOn)
        {
            return string.Format(_message, startedOn.ToShortTimeString());
        }
    }
}