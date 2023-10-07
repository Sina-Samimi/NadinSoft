using Application.DTOs.User;
using AutoMapper;
using Hangfire;
using Infrastructure.EmailFactoryMethod.Contracts;
using Infrastructure.EmailFactoryMethod.ContractsImplementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.DTOs;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<IdentityUser> userManager
            , RoleManager<IdentityRole> roleManager
            , IConfiguration configuration,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            //Check ModelState Validation
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                StringBuilder modelErrors = new StringBuilder();
                foreach (var item in allErrors)
                {
                    modelErrors.Append(item.ErrorMessage);
                }

                Log.Warning(exception: new NullReferenceException(), messageTemplate: modelErrors.ToString());
                modelErrors.AppendLine("لطفا موارد بالا را وارد کنید");
                ResponseDto responseDto = new ResponseDto()
                {
                    Success = false,
                    Message = modelErrors.ToString()
                };

                return Ok(responseDto);
            }
            // Chechk If User Exist
            var ExistUser = await userManager.FindByEmailAsync(registerDto.Email);
            if (ExistUser is not null)
            {
                ResponseDto responseDto = new ResponseDto()
                {
                    Success = false,
                    Message = " این ایمیل قبلا ثبت نام شده است"
                };

                return Ok(responseDto);
            }

            //Map Data And Register User
            var user = mapper.Map<IdentityUser>(registerDto);
            var registerResult = await userManager.CreateAsync(user,user.PasswordHash);
            if (registerResult.Succeeded is false)
            {
                StringBuilder registerErrors = new StringBuilder();
                foreach (var item in registerResult.Errors)
                {
                    registerErrors.AppendLine(item.Description.ToString());
                }
                registerErrors.AppendLine("خطا در ثبت نام");
                Log.Warning("Exception in Register For User:{@Email} With Errors:{@Errors}", user.Email, registerErrors);
                return Ok(new ResponseDto
                {
                    Success = registerResult.Succeeded,
                    Message = registerErrors.ToString()
                });
            }
           
            //var addRoleResult = await userManager.AddToRoleAsync(user, "User");
            //var jobId=jobClient.Enqueue<IEmailSenderFactory>(p => p.Create(configuration["Setting:EmailSenderProvider"]));


            return Ok(new ResponseDto<string>
            {
                Success = true,
                Data = "ثبت نام شما با موفقیت انجام شد"
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin login)
        {
            //Check ModelState Validation
            if (ModelState.IsValid is false)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(e=>e.Errors);
                StringBuilder modelErrors = new StringBuilder();
                foreach (ModelError error in allErrors)
                {
                    modelErrors.AppendLine(error.ToString());
                }

                Log.Warning(exception: new NullReferenceException(), messageTemplate: modelErrors.ToString());
                modelErrors.AppendLine("لطفا موارد بالا را وارد کنید");
                ResponseDto responseDto = new ResponseDto()
                {
                    Success = false,
                    Message = modelErrors.ToString()
                };

                return Ok(responseDto);
            }
         

            //Mapp Data And Check User Login Info
            var userLogin = mapper.Map<IdentityUser>(login);
            StringBuilder loginErrors = new StringBuilder();
            var user = await userManager.FindByEmailAsync(login.Email);
            try
            {
                var checkPassword = await userManager.CheckPasswordAsync(user, login.Password);

                if (user is not null && checkPassword is true)
                {
                    //Log.Information("Login=>User {@Email} Succesfully Logined In", user.Email);

                    return Ok(new ResponseDto<SetToken>
                    {
                        Success = true,
                        Data = new SetToken
                        {
                            message = "توکن اعتبار سنجی",
                            token = new GenerateTokenForUser(configuration).Getoken(user)
                        }
                    });
                }

                //Log.Warning("Exception In Login For User:{@Email} With Errors:{@Errors}", login.Email, "Wrong UserName Or Password");

                return Ok(new ResponseDto
                {
                    Success = false,
                    Message = "اطلاعات ورود شما صحیح نیست"
                });
            }
            catch (Exception e)
            {

                throw;
            }

        }
    }
}
