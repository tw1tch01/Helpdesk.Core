using System;
using AutoMapper;
using Helpdesk.Domain.Enums;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Tickets
{
    public class TicketLookup : IMaps<Ticket>
    {
        public int TicketId { get; set; }
        public Guid Identifier { get; set; }
        public Guid Client { get; set; }
        public Guid? Assignee { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public Severity Severity { get; set; }
        public Priority Priority { get; set; }
        public TicketStatus Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Ticket, TicketLookup>()
                .ForMember(m => m.Client, o => o.MapFrom(m => m.UserGuid))
                .ForMember(m => m.Assignee, o => o.MapFrom(m => m.AssignedUserGuid));
        }
    }
}