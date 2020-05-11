using System;
using Helpdesk.Domain.Entities;
using Todo.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Tickets
{
    public class OpenTicketDto : IMaps<Ticket>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public int? ClientId { get; set; }
        public int? ProjectId { get; set; }
    }
}