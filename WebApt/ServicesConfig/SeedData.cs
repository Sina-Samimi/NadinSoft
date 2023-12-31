﻿using Application.DTOs.User;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Persistance.ConcractsImplementation;
using Persistence.Context;
using WebApi.Utilities;

namespace WebApi.ServicesConfig
{
    public class SeedData
    {
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IServiceCollection _serviceDescriptors;

        public SeedData(
            IServiceCollection serviceDescriptors)
        {

            _serviceDescriptors = serviceDescriptors;
            this.mapper = _serviceDescriptors.BuildServiceProvider().GetService<IMapper>();
            this.userManager = _serviceDescriptors.BuildServiceProvider().GetService<UserManager<IdentityUser>>();
            this.roleManager = _serviceDescriptors.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();

            using (var serviceScope = _serviceDescriptors.BuildServiceProvider().GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<SqlContext>();
                context.Database.EnsureCreated();
            }
            SeedRole();
            SeedUser();

        }

        public void SeedRole()
        {
            var getRole = roleManager.Roles.FirstOrDefault();
            if (getRole is null)
            {
                IdentityRole AdminRole = new IdentityRole()
                {
                    Name = "Admin"
                };

                IdentityRole UserRole = new IdentityRole()
                {
                    Name = "User"
                };

                var setAdminRole = roleManager.CreateAsync(AdminRole).Result;
                var setUserRole = roleManager.CreateAsync(UserRole).Result;
            }
        }
        public void SeedUser()
        {
            var getUser = userManager.Users.FirstOrDefault();
            if (getUser is null)
            {
                var user = new IdentityUser
                {
                     UserName="Sina",
                     Email="sina@gmail.com",
                     PasswordHash="Sina@123"
                };
                var registerResult = userManager.CreateAsync(user, user.PasswordHash).Result;
                var addRoleResult = userManager.AddToRoleAsync(user, "Admin").Result;
            }
        }
       
    }
}
