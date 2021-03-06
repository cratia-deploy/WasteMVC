﻿using Microsoft.EntityFrameworkCore;
using WasteMVC.Models;

namespace WasteMVC.Data
{
    public class SystemContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<WasteType> WasteTypes { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Waste> Wastes { get; set; }

        public SystemContext(DbContextOptions<SystemContext> _options) : base(_options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Waste>()
                .HasOne(w => w.WasteType)
                .WithMany(wt => wt.Wastes)
                .HasForeignKey(w => w.WasteTypeId)
                ;

            modelBuilder.Entity<Partner>()
                .HasOne(pt => pt.Person)
                .WithMany(p => p.Business)
                .HasForeignKey(p => p.PersonId)
                ;

            modelBuilder.Entity<Partner>()
                .HasOne(pt => pt.Waste)
                .WithMany(w => w.Partners)
                .HasForeignKey(p => p.WasteId)
                ;

            modelBuilder.Entity<Person>()
                .HasIndex(x => new { x.FirstName, x.LastName })
                .IsUnique();

            modelBuilder.Entity<WasteType>()
                .HasIndex(x => x.Description)
                .IsUnique()
                ;
        }
    }
}