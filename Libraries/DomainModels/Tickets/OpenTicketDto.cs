using System;
using AutoMapper;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Tickets
{
    public class OpenTicketDto : IMaps<Ticket>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public int ClientId { get; set; }
        public int? ProjectId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OpenTicketDto, Ticket>();
        }
    }
}