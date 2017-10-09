namespace HarbortouchTest.DataContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using HarbortouchTest.Models;

    public partial class HarbortouchContext : DbContext
    {
        public HarbortouchContext()
            : base("name=HarbortouchContext")
        {
        }

        public virtual DbSet<Rental> Rentals { get; set; }
        public virtual DbSet<Tenant> Tenants { get; set; }        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rental>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Rental>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Rental>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Rental>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<Rental>()
                .Property(e => e.OwnerName)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.Cost)
                .HasPrecision(19, 4);
        }
    }
}
