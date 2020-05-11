using AutoMapper;

namespace Todo.DomainModels.Mappings
{
    public interface IMaps<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}