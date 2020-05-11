using System;

namespace Helpdesk.Domain.Common
{
    public abstract class BaseEntity : ICreatedAudit, IModifiedAudit, IIdentity
    {
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedProcess { get; set; }
        public int ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public string ModifiedProcess { get; set; }
        public Guid Identifier { get; set; }
    }
}