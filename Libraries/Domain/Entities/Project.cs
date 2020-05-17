using Helpdesk.Domain.Common;

namespace Helpdesk.Domain.Entities
{
    public class Project : BaseEntity
    {
        public int ProjectId { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        #region Navigational Properties

        public virtual Organization Organization { get; set; }

        #endregion Navigational Properties
    }
}