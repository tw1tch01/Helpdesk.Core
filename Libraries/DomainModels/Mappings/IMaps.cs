using AutoMapper;

namespace Helpdesk.DomainModels.Mappings
{
    public interface IMaps<T>
    {
        void Mapping(Profile profile);
    }
}