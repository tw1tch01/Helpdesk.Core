using AutoMapper;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.TicketLinks
{
    public class LinkTicket : IMaps<TicketLink>
    {
        public int FromTicketId { get; set; }
        public int ToTicketId { get; set; }
        public TicketLinkType LinkType { get; set; }

        #region Methods

        public bool IsSelfLink()
        {
            return FromTicketId == ToTicketId;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LinkTicket, TicketLink>(MemberList.Source);
        }

        #endregion Methods
    }
}