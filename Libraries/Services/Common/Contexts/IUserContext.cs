using Data.Contexts;
using Helpdesk.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services.Common.Contexts
{
    public interface IUserContext : IAuditedContext
    {
        DbSet<User> Users { get; }
    }
}