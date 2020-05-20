using System;
using AutoMapper;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Tickets
{
    public class TicketLookup : IMaps<Ticket>
    {
        public int TicketId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public TicketStatus Status { get; set; }
        public Severity Severity { get; set; }
        public Priority Priority { get; set; }
        //public int LinkedTickets { get; set; }
        public int ClientId { get; set; }
        //public string Client { get; set; }
        public int? ProjectId { get; set; }
        //public string Project { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Ticket, TicketLookup>()
                .ForMember(m => m.TicketId, o => o.MapFrom(m => m.TicketId))
                .ForMember(m => m.Name, o => o.MapFrom(m => m.Name))
                .ForMember(m => m.DueDate, o => o.MapFrom(m => m.DueDate))
                .ForMember(m => m.Status, o => o.MapFrom(m => m.GetStatus()))
                .ForMember(m => m.Severity, o => o.MapFrom(m => m.Severity))
                .ForMember(m => m.Priority, o => o.MapFrom(m => m.Priority))
                .ForMember(m => m.ClientId, o => o.MapFrom(m => m.ClientId))
                .ForMember(m => m.ProjectId, o => o.MapFrom(m => m.ProjectId));
        }
    }
}