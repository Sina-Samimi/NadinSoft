using Application.DTOs.User;
using AutoMapper;
using Castle.Core.Configuration;
using Domain.Entities.Users;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Infrastructure.EmailFactoryMethod.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.DTOs;
using Xunit;

namespace WepApi.Test.Controllers
{
    public class AccountControllerTest
    {

        [Fact]
        public void Register_ShouldRegisterSuccessFully()
        {

            //Arrange
            var userRegister = new UserRegisterDto
            {
                Email = "sina@gmail.com",
                Password = "@Sina123",
                UserName = "sina"
            };

            var user = new IdentityUser()
            {
                UserName = userRegister.UserName,
                Email = userRegister.Email,
                PasswordHash = userRegister.Password
            };

            var res = new IdentityResultDto
            {
                Succeeded=true
            };
            var registerResult = res as IdentityResult;
            var userManagerMoq = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            userManagerMoq.Setup(s =>s.CreateAsync(user, user.PasswordHash)).ReturnsAsync(IdentityResult.Success);
            userManagerMoq.Setup(p=>p.AddToRoleAsync(user,"User")).ReturnsAsync(IdentityResult.Success);


            var roleManagerMoq = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            roleManagerMoq.Setup(p => p.RoleExistsAsync("User").Result).Returns(true);


            var mapperMoq = new Mock<IMapper>();
            mapperMoq.Setup(p => p.Map<IdentityUser>(userRegister)).Returns(user);


            var configurationMoq = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            configurationMoq.Setup(p => p[It.Is<string>(p => p == "Settng:EmailSenderProvider")]).Returns("Gmail");

            //var emailService = new Mock<IEmailSenderFactory>();
            //emailService.Setup(p => p.Create(It.IsAny<string>()));

            var jobClient = new Mock<IBackgroundJobClient>();

            //Act
            AccountController accountController = new AccountController(userManagerMoq.Object, roleManagerMoq.Object, configurationMoq.Object,jobClient.Object, mapperMoq.Object);
            var result = accountController.Register(userRegister).Result;


            //Assert
            //jobClient.Verify(p => p.Create(It.Is<Job>(job => job.Method.Name == "Create"), It.IsAny<EnqueuedState>()));

            OkObjectResult? model = result as OkObjectResult;
            Assert.IsAssignableFrom<ResponseDto<string>>(model?.Value);
            Assert.True(model.StatusCode == 200);
        }
        public class IdentityResultDto : IdentityResult
        {
            public bool Succeeded { get; set; }
            public IEnumerable<IdentityError> Errors { get; set; }
        }

        [Fact]
        public void Login_User_Should_Login()
        {
            //Arrange
            UserLogin userLogin = new UserLogin
            {
                Email = "sina@gmail.com",
                Password = "@Sina123"
            };
            var user = new IdentityUser()
            {
                UserName = "",
                Email = userLogin.Email,
                PasswordHash = userLogin.Password,
            };
            var mapperMoq = new Mock<IMapper>();
            mapperMoq.Setup(p => p.Map<IdentityUser>(userLogin)).Returns(user);

            var userManagerMoq = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            userManagerMoq.Setup(p => p.FindByEmailAsync(userLogin.Email).Result).Returns(user);
            userManagerMoq.Setup(p => p.CheckPasswordAsync(user, userLogin.Password).Result).Returns(true);

            var roleManagerMoq = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);


            var configurationMoq = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            configurationMoq.Setup(p => p[It.Is<string>(p => p == "JWtConfig:Key")]).Returns("16D9BBF8-FA00-4D89-9BB5-99610E95BA70");
            configurationMoq.Setup(p => p[It.Is<string>(p => p == "JWtConfig:issuer")]).Returns("CRUD Project");
            configurationMoq.Setup(p => p[It.Is<string>(p => p == "JWtConfig:audience")]).Returns("Crud Project");
            configurationMoq.Setup(p => p[It.Is<string>(p => p == "JWtConfig:expires")]).Returns("60");
           

            var JobClient = new Mock<IBackgroundJobClient>();
            //JobClient.Setup(p => p.Enqueue<IEmailSenderFactory>(p => p.Create(It.IsAny<string>()))).Returns(It.IsAny<string>);

            //Act
            AccountController accountController = new AccountController(userManagerMoq.Object, roleManagerMoq.Object, configurationMoq.Object,JobClient.Object, mapperMoq.Object);
            var result = accountController.Login(userLogin).Result;


            //Assert
            var model = result as OkObjectResult;
            Assert.IsAssignableFrom<ResponseDto<Application.DTOs.User.SetToken>>(model?.Value);
        }
    }
}
