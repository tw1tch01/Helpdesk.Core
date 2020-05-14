using System;
using AutoMapper;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Tickets
{
    public class OpenTicketDto : IMaps<Ticket>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Severity Severity { get; set; }
        public Priority Priority { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public int ClientId { get; set; }
        public int? ProjectId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OpenTicketDto, Ticket>()
                .ForMember(m => m.Name, o => o.MapFrom(m => m.Name))
                .ForMember(m => m.Description, o => o.MapFrom(m => m.Description))
                .ForMember(m => m.Severity, o => o.MapFrom(m => m.Severity))
                .ForMember(m => m.Priority, o => o.MapFrom(m => m.Priority))
                .ForMember(m => m.DueDate, o => o.MapFrom(m => m.DueDate))
                .ForMember(m => m.ClientId, o => o.MapFrom(m => m.ClientId))
                .ForMember(m => m.ProjectId, o => o.MapFrom(m => m.ProjectId));
        }
    }
}