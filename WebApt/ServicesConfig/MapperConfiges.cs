using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebApi.MappProfiles;

namespace WebApi.ServicesConfig
{
    public class MapperConfiges
    {
        private readonly IServiceCollection services;

        public MapperConfiges(IServiceCollection services)
        {
            this.services = services;
            services.AddAutoMapper(typeof(UserProfile));
            services.AddAutoMapper(typeof(ProductProfile));
        }
       
    }
}
