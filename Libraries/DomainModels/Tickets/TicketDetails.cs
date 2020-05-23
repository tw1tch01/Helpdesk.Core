using System;
using AutoMapper;
using Helpdesk.Domain.Enums;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.Common;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Tickets
{
    public class TicketDetails : IMaps<Ticket>
    {
        public int TicketId { get; set; }
        public Guid TicketGuid { get; set; }
        public CreatedAuditInfo Opened { get; set; }
        public ModifiedAuditInfo Modified { get; set; }
        public Guid Client { get; set; }
        public Guid? Assignee { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public Severity Severity { get; set; }
        public Priority Priority { get; set; }
        public DateTimeOffset? AssignedOn { get; set; }
        public DateTimeOffset? StartedOn { get; set; }
        public DateTimeOffset? PausedOn { get; set; }
        public DateTimeOffset? ResolvedOn { get; set; }
        public DateTimeOffset? ClosedOn { get; set; }
        public TicketStatus Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Ticket, TicketDetails>(MemberList.Destination)
                .ForMember(m => m.TicketGuid, o => o.MapFrom(o => o.Identifier))
                .ForMember(m => m.Client, o => o.MapFrom(m => m.UserGuid))
                .ForMember(m => m.Assignee, o => o.MapFrom(m => m.AssignedUserGuid))
                .ForMember(m => m.Opened, o => o.MapFrom(m => m))
                .ForMember(m => m.Modified, o => o.MapFrom(m => m));
        }
    }
}