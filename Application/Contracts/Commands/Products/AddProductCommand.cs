using Application.DTOs.Product;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Commands.Products
{
    public class AddProductCommand : IRequest<AddProductResultDto>
    {
        private readonly AddProductDto productDto;
        private readonly IdentityUser _userDto;

        public AddProductCommand(AddProductDto productDto, IdentityUser userDto)
        {
            this.productDto = productDto;
            _userDto = userDto;
        }
        public class AddProductQueryHandler : IRequestHandler<AddProductCommand, AddProductResultDto>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper mapper;

            public AddProductQueryHandler(IUnitOfWork unitOfWork
                , IMapper mapper)
            {
                this.unitOfWork = unitOfWork;
                this.mapper = mapper;
            }
            public async Task<AddProductResultDto> Handle(AddProductCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var mapOldProduct = mapper.Map<Product>(request.productDto);
                    mapOldProduct.User = mapper.Map<IdentityUser>(request._userDto);
                    var add = await unitOfWork.GetRepository<Product>().InsertAsync(mapOldProduct);

                    await unitOfWork.SaveChangesAsync();
                    AddProductResultDto? mapNewProduct = mapper.Map<AddProductResultDto>(add.Entity);

                    return mapNewProduct;
                }
                catch (Exception e)
                {

                    Log.Error(e, e.ToString());
                    return await Task.FromException<AddProductResultDto>(e);
                }

            }
        }
    }


}
