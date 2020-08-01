namespace Helpdesk.DomainModels.TicketLinks
{
    public class UnlinkTicket
    {
        public int FromTicketId { get; set; }
        public int ToTicketId { get; set; }

        public bool IsSelfUnlink()
        {
            return FromTicketId == ToTicketId;
        }
    }
}