using System;
using AutoMapper;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Users
{
    public class UserLookup : IMaps<User>
    {
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string DisplayName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserLookup>(MemberList.Destination)
                .ForMember(m => m.UserGuid, o => o.MapFrom(m => m.Identifier))
                .ForMember(m => m.DisplayName, o => o.MapFrom(m => m.GetDisplayName()));
        }
    }
}