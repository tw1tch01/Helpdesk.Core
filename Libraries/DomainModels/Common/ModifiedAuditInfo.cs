using System;
using AutoMapper;
using Helpdesk.Domain.Common;
using Helpdesk.DomainModels.Mappings;

namespace Helpdesk.DomainModels.Common
{
    public class ModifiedAuditInfo : IMaps<IModifiedAudit>
    {
        public string By { get; set; }
        public DateTimeOffset? On { get; set; }
        public string Process { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<IModifiedAudit, ModifiedAuditInfo>()
                .ForMember(m => m.By, o => o.MapFrom(m => m.ModifiedBy))
                .ForMember(m => m.On, o => o.MapFrom(m => m.ModifiedOn))
                .ForMember(m => m.Process, o => o.MapFrom(m => m.ModifiedProcess));
        }
    }
}