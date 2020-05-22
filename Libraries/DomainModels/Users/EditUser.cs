using System;
using System.Collections.Generic;
using AutoMapper;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Common;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Users
{
    public class EditUser : IMaps<User>
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }

        public IReadOnlyDictionary<string, ValueChange> GetChanges(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var changes = new Dictionary<string, ValueChange>();

            if (!string.IsNullOrWhiteSpace(Name) && Name != user.Name) changes.Add(nameof(User.Name), new ValueChange(user.Name, Name));

            if (!string.IsNullOrWhiteSpace(Surname) && Surname != user.Surname) changes.Add(nameof(User.Surname), new ValueChange(user.Surname, Surname));

            if (!string.IsNullOrWhiteSpace(Username) && Username != user.Username) changes.Add(nameof(User.Username), new ValueChange(user.Username, Username));

            return changes;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EditUser, User>();
        }
    }
}