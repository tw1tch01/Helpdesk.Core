using Helpdesk.DomainModels.Organizations;

namespace Helpdesk.DomainModels.Clients
{
    public class SimpleClientDetails
    {
        public int ClientId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public SimpleOrganizationDetails Organization { get; set; }
    }
}