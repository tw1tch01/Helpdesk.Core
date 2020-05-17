using System.Collections.Generic;
using Helpdesk.Domain.Common;

namespace Helpdesk.Domain.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            Tickets = new HashSet<UserTicket>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        #region Navigational Properties

        public virtual ICollection<UserTicket> Tickets { get; private set; }

        #endregion Navigational Properties

        #region Public methods

        public virtual string GetDisplayName()
        {
            return $"{FirstName} {LastName}";
        }

        #endregion Public methods
    }
}