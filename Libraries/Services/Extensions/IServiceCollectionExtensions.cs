using System.Reflection;
using Data.Extensions;
using Helpdesk.DomainModels.Extensions;
using Helpdesk.Services.Notifications;
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
using Helpdesk.Services.Workflows;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Helpdesk.Services.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddDataDependencies();
            services.AddDomainModels();

            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(assembly);

            AddServicesImplementations(services);

            return services;
        }

        private static IServiceCollection AddServicesImplementations(IServiceCollection services)
        {
            #region Tickets

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

            #endregion Tickets

            #region Event Handlers

            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IWorkflowService, WorkflowService>();

            #endregion Event Handlers

            return services;
        }
    }
}