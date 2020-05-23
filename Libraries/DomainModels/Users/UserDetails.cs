using System;
using AutoMapper;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Common;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Users
{
    public class UserDetails : IMaps<User>
    {
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        public CreatedAuditInfo Created { get; set; }
        public ModifiedAuditInfo Modified { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDetails>()
                .ForMember(m => m.UserGuid, o => o.MapFrom(m => m.Identifier))
                .ForMember(m => m.Created, o => o.MapFrom(m => m))
                .ForMember(m => m.Modified, o => o.MapFrom(m => m));
        }
    }
}