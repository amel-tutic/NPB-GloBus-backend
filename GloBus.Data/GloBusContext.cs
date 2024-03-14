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
        public DbSet<Ticket> Ticket{ get; set; }
        public DbSet<TicketType> TicketType { get; set; }
        public DbSet<Line> Line { get; set; }
        public DbSet<ActiveTickets> ActiveTickets { get; set; }
        public DbSet<InvalidTickets> InvalidTickets { get; set; }
        public DbSet<Penalty> Penalty { get; set; }
        public DbSet<Admin> Admins { get; set; }
      /*  public DbSet<Region> Regions { get; set; }*/


    }
}
