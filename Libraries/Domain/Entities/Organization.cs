using System.Collections.Generic;
using Helpdesk.Domain.Common;

namespace Helpdesk.Domain.Entities
{
    public class Organization : BaseEntity
    {
        public Organization()
        {
            Members = new HashSet<Client>();
            Projects = new HashSet<Project>();
        }

        public int OrganizationId { get; set; }
        public string Name { get; set; }

        #region Navigational Properties

        public virtual ICollection<Client> Members { get; private set; }
        public virtual ICollection<Project> Projects { get; private set; }

        #endregion Navigational Properties
    }
}