using System;
using System.Collections.Generic;
using Helpdesk.Domain.Enums;
using Helpdesk.DomainModels.Tickets.Enums;

namespace Helpdesk.DomainModels.Tickets
{
    public class TicketLookupParams
    {
        public DateTimeOffset? CreatedAfter { get; set; }
        public DateTimeOffset? CreatedBefore { get; set; }
        public string SearchBy { get; set; }
        public IList<int> TicketIds { get; set; } = new List<int>();
        public TicketStatus? FilterByStatus { get; set; }
        public Severity? FilterBySeverity { get; set; }
        public Priority? FilterByPriority { get; set; }
        public SortTicketsBy? SortBy { get; set; }
    }
}