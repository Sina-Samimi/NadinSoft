using Hangfire;

namespace WebApi.ServicesConfig
{
    public class HangfireConfigs
    {
        private readonly IServiceCollection services;
        private readonly IConfiguration configuration;

        public HangfireConfigs(IServiceCollection services
            , IConfiguration configuration)
        {
            this.services = services;
            this.configuration = configuration;


            services.AddHangfire(config =>
                 config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                 .UseSimpleAssemblyNameTypeSerializer()
                 .UseRecommendedSerializerSettings()
                 .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

            services.AddHangfireServer();
        }
    }
}
