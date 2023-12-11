using GloBus_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace GloBus_backend.Infrastructure
{
    public class GloBusContext : DbContext
    {
        public GloBusContext(DbContextOptions<GloBusContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    }
}
