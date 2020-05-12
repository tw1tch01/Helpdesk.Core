using System;
using AutoMapper;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.UserTickets
{
    public class AssignedUserDetails : IMaps<UserTicket>
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public DateTimeOffset AssignedOn { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserTicket, AssignedUserDetails>()
                .ForMember(m => m.UserId, o => o.MapFrom(m => m.UserId))
                .ForMember(m => m.DisplayName, o => o.MapFrom(m => m.User.GetDisplayName()))
                .ForMember(m => m.AssignedOn, o => o.MapFrom(m => m.CreatedOn));
        }
    }
}