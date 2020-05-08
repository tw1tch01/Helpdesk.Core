using System;

namespace Domain.Common
{
    public interface IModifiedAudit
    {
        string ModifiedBy { get; set; }
        DateTime ModifiedOn { get; set; }
        string ModifiedProcess { get; set; }
    }
}