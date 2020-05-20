using System;
using AutoMapper;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.DomainModels.Common;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Tickets
{
    public class FullTicketDetails : IMaps<Ticket>
    {
        public int TicketId { get; set; }
        public CreatedAuditInfo Created { get; set; }
        public ModifiedAuditInfo Modified { get; set; }
        public Guid Identifier { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public Severity Severity { get; set; }
        public Priority Priority { get; set; }
        public TicketStatus Status { get; set; }
        public DateTimeOffset? StartedOn { get; set; }
        public DateTimeOffset? PausedOn { get; set; }
        //public UserActionDetails UserAction { get; set; }
        //public SimpleClientDetails Client { get; set; }
        //public SimpleProjectDetails Project { get; set; }
        //public IList<AssignedUserDetails> Assignees { get; set; } = new List<AssignedUserDetails>();
        //public IList<TicketLinkDetails> LinkedTickets { get; set; } = new List<TicketLinkDetails>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Ticket, FullTicketDetails>(MemberList.Destination)
                .ForMember(m => m.Created, o => o.MapFrom(m => m))
                .ForMember(m => m.Modified, o => o.MapFrom(m => m));
        }
    }
}