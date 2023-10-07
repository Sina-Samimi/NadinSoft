using Application.DTOs;
using Application.DTOs.Product;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApi.Helpers
{
    public class ProductAuthorizationHandler : AuthorizationHandler<ProductAuthorRequirement, AuthorizationRquirmentDto<GetProductByIdDto>>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProductAuthorRequirement requirement, AuthorizationRquirmentDto<GetProductByIdDto> resource)
        {
            if (context.User.Claims.Single(p=>p.Type=="Name").Value == resource.Dto.User.UserName)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
    

    //public class IsProductRequirmentAuthorization : AuthorizationHandler<ProductRequirment, >
    //{
    //    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProductRequirment requirement, AuthorizationRquirmentDto<AddProductDto> resource)
    //    {
    //        if (context.User.Identity?.Name == resource.Dto.Name)
    //        {
    //            context.Succeed(requirement);
    //        }

    //        return Task.CompletedTask;
    //    }
    //}
    public class ProductAuthorRequirement : IAuthorizationRequirement { }
}
