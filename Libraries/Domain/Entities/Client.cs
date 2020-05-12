using System;
using System.Collections.Generic;
using Helpdesk.Domain.Common;

namespace Helpdesk.Domain.Entities
{
    public class Client : BaseEntity
    {
        public Client()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int ClientId { get; set; }
        public int OrganizationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        #region Navigational Properties

        public virtual Organization Organization { get; set; }
        public virtual ICollection<Ticket> Tickets { get; private set; }

        public object GetDisplayName()
        {
            throw new NotImplementedException();
        }

        #endregion Navigational Properties
    }
}