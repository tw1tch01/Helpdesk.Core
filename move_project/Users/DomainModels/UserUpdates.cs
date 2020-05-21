using System;
using System.Collections.Generic;
using AutoMapper;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Common;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Users
{
    public class UserUpdates : IMaps<User>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public IReadOnlyDictionary<string, ValueChange> GetChanges(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var changes = new Dictionary<string, ValueChange>();

            if (!string.IsNullOrWhiteSpace(FirstName) && FirstName != user.FirstName) changes.Add(nameof(User.FirstName), new ValueChange(user.FirstName, FirstName));

            if (!string.IsNullOrWhiteSpace(LastName) && LastName != user.LastName) changes.Add(nameof(User.LastName), new ValueChange(user.LastName, LastName));

            if (!string.IsNullOrWhiteSpace(Username) && Username != user.Username) changes.Add(nameof(User.Username), new ValueChange(user.Username, Username));

            return changes;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserUpdates, User>();
        }
    }
}