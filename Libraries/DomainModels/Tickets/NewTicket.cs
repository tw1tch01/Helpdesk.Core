using System;
using AutoMapper;
using Helpdesk.Domain.Enums;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Tickets
{
    public class NewTicket : IMaps<Ticket>
    {
        public Guid Client { get; set; }
        public Guid? Assignee { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public Severity Severity { get; set; }
        public Priority Priority { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewTicket, Ticket>(MemberList.Source)
                .ForMember(m => m.UserGuid, o => o.MapFrom(m => m.Client))
                .ForMember(m => m.AssignedUserGuid, o => o.MapFrom(m => m.Assignee))
                .ForMember(m => m.Name, o => o.MapFrom(m => m.Name))
                .ForMember(m => m.Description, o => o.MapFrom(m => m.Description))
                .ForMember(m => m.DueDate, o => o.MapFrom(m => m.DueDate))
                .ForMember(m => m.Severity, o => o.MapFrom(m => m.Severity))
                .ForMember(m => m.Priority, o => o.MapFrom(m => m.Priority));
        }
    }
}