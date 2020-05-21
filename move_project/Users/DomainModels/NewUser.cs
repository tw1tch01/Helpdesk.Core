using AutoMapper;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Users
{
    public class NewUser : IMaps<User>
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewUser, User>();
        }
    }
}