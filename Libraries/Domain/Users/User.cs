using Helpdesk.Domain.Common;

namespace Helpdesk.Domain.Users
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }

        #region Public Methods

        public string GetDisplayName()
        {
            if (!string.IsNullOrEmpty(Alias)) return Alias;

            return $"{Name} {Surname}";
        }

        #endregion Public Methods
    }
}