using Application;
using Application.Contracts;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog.Sinks.MSSqlServer;
using System.Configuration;
using System.Text;
using WebApi.Helpers;
using WebApi.ServicesConfig;
using Infrastructure.EmailFactoryMethod.Contracts;
using Infrastructure.EmailFactoryMethod.ContractsImplementation;
using MediatR;
using Application.Contracts.Commands.Products;
using AutoMapper;
using Application.DTOs.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Persistance.ConcractsImplementation;

var builder = WebApplication.CreateBuilder(args);

#region DbContext & IdentityConfigs
string sqlConnection = builder.Configuration["ConnectionStrings:SqlConnectionString"];
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<SqlContext>(opt => opt.UseSqlServer(sqlConnection));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<SqlContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<CustomIdentityError>();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 3;
});
#endregion


#region Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion


#region AddAuthontication & JWT Configs
JWTConfigs jWTConfigs = new JWTConfigs(builder.Services, builder.Configuration);
#endregion


#region AutomaperProfiles
MapperConfiges mapperConfiges = new MapperConfiges(builder.Services);
#endregion


#region HangfireConfig
HangfireConfigs HangfireConfigs = new HangfireConfigs(builder.Services, builder.Configuration);
#endregion


#region MediatRConfig
MediatRConfigs mediatRConfigs = new MediatRConfigs(builder.Services);
#endregion


#region SerilogConfigs
SerilogConfigs SerilogConfigs = new SerilogConfigs();
SerilogConfigs.UseSeq();
builder.Host.UseSerilog();
#endregion


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
SeedData SeedData = new SeedData(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.UseHangfireDashboard("/hangfire", new DashboardOptions
//{
//    //Authorization = new[]
//    //  {
//    //       new HangfireAuthorizationFilter(),
//    //  },
//    DashboardTitle = "پنل مدیریت Hangfire",
//});
app.Run();
