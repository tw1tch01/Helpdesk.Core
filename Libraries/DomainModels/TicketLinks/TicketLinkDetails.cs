using System;
using AutoMapper;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.TicketLinks
{
    public class TicketLinkDetails : IMaps<TicketLink>
    {
        public int FromTicketId { get; set; }
        public string FromTicketName { get; set; }
        public int ToTicketId { get; set; }
        public string ToTicketName { get; set; }
        public TicketLinkType LinkType { get; set; }
        public DateTimeOffset LinkedOn { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TicketLink, TicketLinkDetails>()
                .ForMember(m => m.FromTicketId, o => o.MapFrom(m => m.FromTicketId))
                .ForMember(m => m.FromTicketName, o => o.MapFrom(m => m.FromTicket.Name))
                .ForMember(m => m.ToTicketId, o => o.MapFrom(m => m.ToTicketId))
                .ForMember(m => m.ToTicketName, o => o.MapFrom(m => m.ToTicket.Name))
                .ForMember(m => m.LinkType, o => o.MapFrom(m => m.LinkType))
                .ForMember(m => m.LinkedOn, o => o.MapFrom(m => m.CreatedOn));
        }
    }
}