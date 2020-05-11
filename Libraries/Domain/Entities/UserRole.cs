using System;
using Domain.Common;

namespace Domain.Entities
{
    public class UserRole : ICreatedAudit
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedProcess { get; set; }

        #region Navigational Properties

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

        #endregion Navigational Properties
    }
}