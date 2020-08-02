using Data.Extensions;
using Helpdesk.DomainModels.Extensions;
using Helpdesk.Services.TicketLinks.Commands.LinkTickets;
using Helpdesk.Services.TicketLinks.Commands.UnlinkTickets;
using Helpdesk.Services.TicketLinks.Factories.LinkTickets;
using Helpdesk.Services.TicketLinks.Factories.UnlinkTickets;
using Helpdesk.Services.Tickets.Commands.CloseTicket;
using Helpdesk.Services.Tickets.Commands.DeleteTicket;
using Helpdesk.Services.Tickets.Commands.OpenTicket;
using Helpdesk.Services.Tickets.Commands.PauseTicket;
using Helpdesk.Services.Tickets.Commands.ReopenTicket;
using Helpdesk.Services.Tickets.Commands.ResolveTicket;
using Helpdesk.Services.Tickets.Commands.StartTicket;
using Helpdesk.Services.Tickets.Commands.UpdateTicket;
using Helpdesk.Services.Tickets.Factories.CloseTicket;
using Helpdesk.Services.Tickets.Factories.DeleteTicket;
using Helpdesk.Services.Tickets.Factories.OpenTicket;
using Helpdesk.Services.Tickets.Factories.PauseTicket;
using Helpdesk.Services.Tickets.Factories.ReopenTicket;
using Helpdesk.Services.Tickets.Factories.ResolveTicket;
using Helpdesk.Services.Tickets.Factories.StartTicket;
using Helpdesk.Services.Tickets.Factories.UpdateTicket;
using Helpdesk.Services.Tickets.Queries.GetTicket;
using Helpdesk.Services.Tickets.Queries.LookupTickets;
using Helpdesk.Services.Users.Commands.CreateUser;
using Helpdesk.Services.Users.Commands.DeleteUser;
using Helpdesk.Services.Users.Commands.UpdateUser;
using Helpdesk.Services.Users.Factories.CreateUser;
using Helpdesk.Services.Users.Factories.DeleteUser;
using Helpdesk.Services.Users.Factories.UpdateUser;
using Microsoft.Extensions.DependencyInjection;

namespace Helpdesk.Services.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddDataDependencies();
            services.AddDomainModels();

            AddTicketServices(services);
            AddTicketLinkServices(services);
            AddUserServices(services);

            return services;
        }

        private static IServiceCollection AddTicketServices(IServiceCollection services)
        {
            // Commands
            services.AddTransient<ICloseTicketService, CloseTicketService>();
            services.AddTransient<IDeleteTicketService, DeleteTicketService>();
            services.AddTransient<IOpenTicketService, OpenTicketService>();
            services.AddTransient<IPauseTicketService, PauseTicketService>();
            services.AddTransient<IReopenTicketService, ReopenTicketService>();
            services.AddTransient<IResolveTicketService, ResolveTicketService>();
            services.AddTransient<IStartTicketService, StartTicketService>();
            services.AddTransient<IUpdateTicketService, UpdateTicketService>();

            // Factories
            services.AddTransient<ICloseTicketResultFactory, CloseTicketResultFactory>();
            services.AddTransient<IDeleteTicketResultFactory, DeleteTicketResultFactory>();
            services.AddTransient<IOpenTicketResultFactory, OpenTicketResultFactory>();
            services.AddTransient<IPauseTicketResultFactory, PauseTicketResultFactory>();
            services.AddTransient<IReopenTicketResultFactory, ReopenTicketResultFactory>();
            services.AddTransient<IResolveTicketResultFactory, ResolveTicketResultFactory>();
            services.AddTransient<IStartTicketResultFactory, StartTicketResultFactory>();
            services.AddTransient<IUpdateTicketResultFactory, UpdateTicketResultFactory>();

            // Queries
            services.AddTransient<IGetTicketService, GetTicketService>();
            services.AddTransient<ILookupTicketsService, LookupTicketsService>();

            return services;
        }

        private static IServiceCollection AddTicketLinkServices(IServiceCollection services)
        {
            // Commands
            services.AddTransient<ILinkTicketService, LinkTicketService>();
            services.AddTransient<IUnlinkTicketService, UnlinkTicketService>();

            // Factories
            services.AddTransient<ILinkTicketsResultFactory, LinkTicketsResultFactory>();
            services.AddTransient<IUnlinkTicketsResultFactory, UnlinkTicketsResultFactory>();

            return services;
        }

        private static IServiceCollection AddUserServices(IServiceCollection services)
        {
            // Commands
            services.AddTransient<ICreateUserService, CreateUserService>();
            services.AddTransient<IDeleteUserService, DeleteUserService>();
            services.AddTransient<IUpdateUserService, UpdateUserService>();

            // Factories
            services.AddTransient<ICreateUserResultFactory, CreateUserResultFactory>();
            services.AddTransient<IDeleteUserResultFactory, DeleteUserResultFactory>();
            services.AddTransient<IUpdateUserResultFactory, UpdateUserResultFactory>();

            return services;
        }
    }
}