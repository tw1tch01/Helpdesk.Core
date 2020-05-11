using System.Collections.Generic;

namespace Domain.Entities
{
    public class Role
    {
        public Role()
        {
            Privileges = new HashSet<RolePrivilege>();
            Users = new HashSet<UserRole>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        #region Navigational Properties

        public virtual ICollection<RolePrivilege> Privileges { get; private set; }
        public virtual ICollection<UserRole> Users { get; private set; }

        #endregion Navigational Properties
    }
}