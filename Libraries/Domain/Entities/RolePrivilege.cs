using System;
using Domain.Common;

namespace Domain.Entities
{
    public class RolePrivilege : ICreatedAudit
    {
        public int RoleId { get; set; }
        public int PrivilegeId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedProcess { get; set; }

        #region Navigational Properties

        public virtual Role Role { get; set; }
        public virtual Privilege Privilege { get; set; }

        #endregion Navigational Properties
    }
}