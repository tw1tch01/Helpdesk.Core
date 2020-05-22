using AutoMapper;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Users
{
    public class NewUser : IMaps<User>
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewUser, User>();
        }
    }
}