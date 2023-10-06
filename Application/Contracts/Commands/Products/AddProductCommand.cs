using Application.DTOs.Product;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;
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

        public AddProductCommand(AddProductDto productDto)
        {
            this.productDto = productDto;
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
