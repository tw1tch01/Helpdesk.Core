using System;

namespace Helpdesk.Domain.Common
{
    public interface ICreatedAudit
    {
        int CreatedBy { get; set; }
        DateTimeOffset CreatedOn { get; set; }
        string CreatedProcess { get; set; }
    }
}