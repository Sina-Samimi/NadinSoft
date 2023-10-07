using Application.DTOs.Product;
using AutoMapper;
using Domain.Entities.Products;

namespace WebApi.MappProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, GetAllProductsResponse>()
                .ReverseMap();

            CreateMap<Product, AddProductDto>()
                .ReverseMap();

            CreateMap<Product, AddProductResultDto>()
                .ReverseMap();

            CreateMap<Product, UpdateProductDto>()
                .ReverseMap();

            CreateMap<Product, GetProductByIdDto>()
                .ReverseMap();
        }
    }
}
