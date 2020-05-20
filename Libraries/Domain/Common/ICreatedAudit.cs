using System;

namespace Helpdesk.Domain.Common
{
    public interface ICreatedAudit
    {
        string CreatedBy { get; set; }

        DateTimeOffset CreatedOn { get; set; }

        string CreatedProcess { get; set; }
    }
}