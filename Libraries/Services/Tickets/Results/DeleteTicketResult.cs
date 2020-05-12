using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Results
{
    public class DeleteTicketResult
    {
        public DeleteTicketResult(TicketDeleteResult result)
        {
            Result = result;
        }

        public TicketDeleteResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; set; }

        internal static DeleteTicketResult TicketNotFound(int ticketId)
        {
            return new DeleteTicketResult(TicketDeleteResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        internal static DeleteTicketResult Deleted(int ticketId)
        {
            return new DeleteTicketResult(TicketDeleteResult.Deleted)
            {
                TicketId = ticketId
            };
        }

        private string GetMessage()
        {
            return Result switch
            {
                TicketDeleteResult.Deleted => TicketResultMessages.Deleted,
                TicketDeleteResult.TicketNotFound => TicketResultMessages.TicketNotFound,
                _ => Result.ToString()
            };
        }
    }
}