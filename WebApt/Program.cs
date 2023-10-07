using Application;
using Application.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Serilog;
using WebApi.Helpers;
using WebApi.ServicesConfig;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

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

#region Authorization&Plicis
builder.Services.AddAuthorization(op =>
{
    op.AddPolicy("IsProductForUser", policy => {
        policy.Requirements.Add(new ProductAuthorRequirement());
    });
});
#endregion

#region Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IAuthorizationHandler, ProductAuthorizationHandler>();
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

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
       new OpenApiInfo
       {
           Title = "نادین سافت",
           Version = "v1"
       });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "احراز هویت با استفاده از Jwt",
        Name = "احراز هویت شرکت",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",

    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });

    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmplFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
    c.IncludeXmlComments(xmplFullPath);
});

//SeedData SeedData = new SeedData(builder.Services);

var app = builder.Build();
app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NadinSoft V1");
        c.InjectStylesheet("/css/swaggerrtl.css");
    });
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
