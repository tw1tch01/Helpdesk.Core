using System;

namespace Helpdesk.Domain.Common
{
    public interface IModifiedAudit
    {
        string ModifiedBy { get; set; }

        DateTimeOffset? ModifiedOn { get; set; }

        string ModifiedProcess { get; set; }
    }
}