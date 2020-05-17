using System;
using System.Collections.Generic;
using AutoMapper;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.DomainModels.Mappings;
using Helpdesk.DomainModels.UserTickets;

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
        public IList<AssignedUserDetails> Assignees { get; set; } = new List<AssignedUserDetails>();
        public int LinkedTickets { get; set; }
        public int ClientId { get; set; }
        public string Client { get; set; }
        public int? ProjectId { get; set; }
        public string Project { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Ticket, TicketLookup>()
                .ForMember(m => m.TicketId, o => o.MapFrom(m => m.TicketId))
                .ForMember(m => m.Name, o => o.MapFrom(m => m.Name))
                .ForMember(m => m.DueDate, o => o.MapFrom(m => m.DueDate))
                .ForMember(m => m.Status, o => o.MapFrom(m => m.GetStatus()))
                .ForMember(m => m.Severity, o => o.MapFrom(m => m.Severity))
                .ForMember(m => m.Priority, o => o.MapFrom(m => m.Priority))
                .ForMember(m => m.Assignees, o => o.MapFrom(m => m.AssignedUsers))
                .ForMember(m => m.LinkedTickets, o => o.MapFrom(m => m.LinkedTickets.Count))
                .ForMember(m => m.ClientId, o => o.MapFrom(m => m.ClientId))
                .ForMember(m => m.Client, o => o.Condition(m => m.Client != null))
                .ForMember(m => m.Client, o => o.MapFrom(m => $"{m.Client.FirstName} {m.Client.LastName}"))
                .ForMember(m => m.ProjectId, o => o.MapFrom(m => m.ProjectId))
                .ForMember(m => m.Project, o => o.Condition(m => m.Project != null))
                .ForMember(m => m.Project, o => o.MapFrom(m => m.Project.Name))
                //.ForMember(m => m.OrganizationId , o => o.MapFrom(m => m.OrganizationId))
                //.ForMember(m => m.Organization , o => o.Condition(m => m.Organization != null))
                //.ForMember(m => m.Organization , o => o.MapFrom(m => m.Organization.Name))
                ;
        }
    }
}