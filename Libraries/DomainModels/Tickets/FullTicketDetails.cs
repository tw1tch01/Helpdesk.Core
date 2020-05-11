using System;
using AutoMapper;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.DomainModels.Clients;
using Helpdesk.DomainModels.Common;
using Helpdesk.DomainModels.Organizations;
using Helpdesk.DomainModels.Projects;
using Todo.DomainModels.Mappings;

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
        public SimpleClientDetails Client { get; set; }
        public SimpleProjectDetails Project { get; set; }
        public SimpleOrganizationDetails Organization { get; set; }

        public void Mapping(Profile profile)
        {
        }
    }
}