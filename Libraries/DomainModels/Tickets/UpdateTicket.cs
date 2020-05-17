using System;
using System.Collections.Generic;
using AutoMapper;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.DomainModels.Common;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Tickets
{
    public class UpdateTicketDto : IMaps<Ticket>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public bool UpdateDueDate { get; set; }
        public Severity? Severity { get; set; }
        public Priority? Priority { get; set; }

        public IReadOnlyDictionary<string, ValueChange> GetChanges(Ticket ticket)
        {
            if (ticket == null) throw new ArgumentNullException(nameof(ticket));

            var changes = new Dictionary<string, ValueChange>();

            if (!string.IsNullOrWhiteSpace(Name) && Name != ticket.Name) changes.Add(nameof(Ticket.Name), new ValueChange(ticket.Name, Name));

            if (!string.IsNullOrWhiteSpace(Description) && Description != ticket.Description) changes.Add(nameof(Ticket.Description), new ValueChange(ticket.Name, Description));

            if (UpdateDueDate && DueDate != ticket.DueDate) changes.Add(nameof(Ticket.DueDate), new ValueChange(ticket.DueDate, DueDate));

            if (Severity.HasValue && ticket.Severity != Severity) changes.Add(nameof(Ticket.Severity), new ValueChange(ticket.Severity, Severity.Value));

            if (Priority.HasValue && ticket.Priority != Priority) changes.Add(nameof(Ticket.Priority), new ValueChange(ticket.Priority, Priority.Value));

            return changes;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateTicketDto, Ticket>(MemberList.None)
                .ForMember(m => m.Name, o => o.Condition(c => !string.IsNullOrWhiteSpace(c.Name)))
                .ForMember(m => m.Description, o => o.Condition(c => !string.IsNullOrWhiteSpace(c.Description)))
                .ForMember(m => m.DueDate, o => o.Condition(c => c.UpdateDueDate))
                .ForMember(m => m.Severity, o => o.Condition(c => c.Severity.HasValue))
                .ForMember(m => m.Priority, o => o.Condition(c => c.Priority.HasValue));
        }
    }
}