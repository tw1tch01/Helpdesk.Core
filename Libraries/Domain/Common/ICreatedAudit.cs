using System;

namespace Domain.Common
{
    public interface ICreatedAudit
    {
        string CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        string CreatedProcess { get; set; }
    }
}