namespace Helpdesk.DomainModels.TicketLinks
{
    public class RemoveTicketsLink
    {
        public int FromTicketId { get; set; }
        public int ToTicketId { get; set; }

        public bool IsSelfUnlink()
        {
            return FromTicketId == ToTicketId;
        }
    }
}