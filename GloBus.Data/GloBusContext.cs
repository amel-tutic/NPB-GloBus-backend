using GloBus.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GloBus.Data
{
    public class GloBusContext : DbContext
    {
        public GloBusContext(DbContextOptions<GloBusContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    }
}
