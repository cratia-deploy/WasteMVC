using Microsoft.EntityFrameworkCore;
using WasteMVC.Models;

namespace WasteMVC.Data
{
    public class SystemContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public SystemContext(DbContextOptions<SystemContext> _options) : base(_options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasIndex(x => new { x.FirstName, x.LastName })
                .IsUnique();
        }
    }
}