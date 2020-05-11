namespace Helpdesk.Domain.Exceptions.Tickets
{
    public class TicketNotStartedException : TicketException
    {
        private const string _message = "Ticket has not been started yet.";

        public TicketNotStartedException(int ticketId)
            : base(ticketId, _message)
        {
        }
    }
}