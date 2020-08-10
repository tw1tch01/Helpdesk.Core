using AutoMapper;
using FluentValidation;
using Helpdesk.DomainModels.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Helpdesk.DomainModels.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomainModels(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            #region Automapper

            services.AddAutoMapper(assembly);
            services.AddSingleton(opt => new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            }).CreateMapper());

            #endregion Automapper

            #region FluentValidation

            services.AddValidatorsFromAssembly(assembly);

            #endregion FluentValidation

            return services;
        }
    }
}