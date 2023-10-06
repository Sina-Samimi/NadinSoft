using Application.Contracts.Queries.Products;
using MediatR;

namespace WebApi.ServicesConfig
{
    public class MediatRConfigs
    {
        private readonly IServiceCollection services;

        public MediatRConfigs(IServiceCollection services)
        {
            this.services = services;
            services.AddMediatR(typeof(GetAllProductsCommand).Assembly);
        }
       
    }
}
