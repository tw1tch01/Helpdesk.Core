using System;

namespace Helpdesk.Domain.Common
{
    public interface IModifiedAudit
    {
        int? ModifiedBy { get; set; }
        DateTimeOffset? ModifiedOn { get; set; }
        string ModifiedProcess { get; set; }
    }
}