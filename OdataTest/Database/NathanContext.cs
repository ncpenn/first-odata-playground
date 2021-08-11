using System;
using Microsoft.EntityFrameworkCore;
using OdataTest.Models;
using OdataTest.Models.ProductService.Models;

namespace OdataTest.Database
{
    public class NathanContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Accessory> Accessories { get; set; }

        public NathanContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            //base.OnModelCreating(mb);

            //mb.Entity<Product>();

        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("NathanDb");
        }
    }
}
