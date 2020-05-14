using System;
using AutoMapper;
using Helpdesk.Domain.Common;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Common
{
    public class CreatedAuditInfo : IMaps<ICreatedAudit>
    {
        public int By { get; set; }
        public DateTimeOffset On { get; set; }
        public string Process { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ICreatedAudit, CreatedAuditInfo>()
                .ForMember(m => m.By, o => o.MapFrom(m => m.CreatedBy))
                .ForMember(m => m.On, o => o.MapFrom(m => m.CreatedOn))
                .ForMember(m => m.Process, o => o.MapFrom(m => m.CreatedProcess));
        }
    }
}