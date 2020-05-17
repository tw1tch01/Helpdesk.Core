using Data.Contexts;
using Helpdesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services.Common
{
    public interface ITicketContext : IAuditedContext
    {
        DbSet<Ticket> Tickets { get; }
    }
}