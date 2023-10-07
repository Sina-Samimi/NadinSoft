using AutoMapper;
using Domain.Entities.Products;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Persistance.ConcractsImplementation;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Context
{
    public class SqlContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {

        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
       

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>().Property(p => p.ManufactureEmail)
               .HasMaxLength(50);

            builder.Entity<Product>().Property(p => p.Name)
                .HasMaxLength(30);

            builder.Entity<Product>().Property(p => p.ManufacturePhone)
              .HasMaxLength(11);

            builder.Entity<Product>()
                .HasIndex(p => p.ManufacturePhone)
                .IsUnique();

            builder.Entity<Product>()
                .HasIndex(p => p.ManufactureEmail)
                .IsUnique();



            base.OnModelCreating(builder);
        }
        public DbSet<Product> Products { get; set; }
    }
}
