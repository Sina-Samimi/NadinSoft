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


        #region SeedData
        private string? SeedRole()
        {
            var getRole = this.Roles.FirstOrDefault();
            if (getRole is null)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = "Admin"
                };
                var setRole = this.Roles.AddAsync(identityRole).Result;
                this.SaveChanges();
                return setRole.Entity.Id;
            }
            return null;

        }
        private void SeedUser(string roleId)
        {
            var getUser = this.Users.FirstOrDefault();
            if (getUser is null)
            {
                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                var user = new User()
                {
                    Email = "sina@gmail.com",
                    UserName = "Sina",
                    PasswordHash = "Sina@123"
                };

                var pass = passwordHasher.HashPassword(user, "Sina@123");

                var newUser = new IdentityUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PasswordHash = pass,
                    NormalizedEmail = user.Email.ToUpper(),
                    NormalizedUserName = user.UserName.ToUpper()
                };

                this.Users.Add(newUser);


                var addRoleResult = this.UserRoles.AddAsync(new IdentityUserRole<string>
                {
                    RoleId = roleId,
                    UserId = newUser.Id

                }).Result;

                this.SaveChanges();
            }

        }

        #endregion

    }
}
