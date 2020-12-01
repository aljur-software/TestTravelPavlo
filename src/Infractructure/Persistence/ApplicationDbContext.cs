using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infractructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Agent> Agents { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            Database.EnsureCreated();
        }
    }
}