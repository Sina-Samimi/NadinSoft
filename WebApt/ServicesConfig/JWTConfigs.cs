using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace WebApi.ServicesConfig
{
    public class JWTConfigs
    {
        private readonly IServiceCollection services;
        private readonly IConfiguration configuration;

        public JWTConfigs(IServiceCollection services
            ,IConfiguration configuration)
        {
            this.services = services;
            this.configuration = configuration;
            services.AddAuthentication(Options =>
            {
                Options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["JWTConfig:issuer"],
                    ValidAudience = configuration["JWTConfig:audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTConfig:Key"])),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                };
                options.SaveToken = true; // HttpContext.GetTokenAsunc();
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //log

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        //log
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        return Task.CompletedTask;

                    },
                    OnMessageReceived = context =>
                    {
                        return Task.CompletedTask;

                    },
                    OnForbidden = context =>
                    {
                        return Task.CompletedTask;

                    }
                };
            }
);
        }

    }
}
