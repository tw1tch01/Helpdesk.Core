using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Privilege : ICreatedAudit, IIdentity
    {
        public Privilege()
        {
            Roles = new HashSet<RolePrivilege>();
        }

        public int PrivilegeId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedProcess { get; set; }
        public Guid Identifier { get; set; }
        public string Name { get; set; }
        public string UniqueName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        #region Navigational Properties

        public virtual ICollection<RolePrivilege> Roles { get; private set; }

        #endregion Navigational Properties
    }
}