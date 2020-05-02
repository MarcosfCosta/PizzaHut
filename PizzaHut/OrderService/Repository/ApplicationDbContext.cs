using Microsoft.EntityFrameworkCore;
using OrderService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) :
            base(options)
        {
        }

        public DbSet<Order> Order { get; set; }
        //public DbSet<BolsaValores> BolsasValores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Indicador>()
            //    .HasKey(i => i.Sigla);
            //modelBuilder.Entity<BolsaValores>()
            //    .HasKey(b => b.Sigla);
        }
    }
}
