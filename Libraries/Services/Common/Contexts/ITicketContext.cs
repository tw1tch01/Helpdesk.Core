using Data.Contexts;
using Helpdesk.Domain.Tickets;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services.Common.Contexts
{
    public interface ITicketContext : IAuditedContext
    {
        DbSet<Ticket> Tickets { get; }
        DbSet<TicketLink> TicketLinks { get; }
    }
}