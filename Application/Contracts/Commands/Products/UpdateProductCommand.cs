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
    public class UpdateProductCommand:IRequest<UpdateProductDto>
    {
        private readonly UpdateProductDto updateProductDto;

        public UpdateProductCommand(UpdateProductDto updateProductDto)
        {
            this.updateProductDto = updateProductDto;
        }
        public class UpdateProductCommadnHandler : IRequestHandler<UpdateProductCommand, UpdateProductDto>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper mapper;

            public UpdateProductCommadnHandler(IUnitOfWork unitOfWork
                ,IMapper mapper)
            {
                this.unitOfWork = unitOfWork;
                this.mapper = mapper;
            }
            public async Task<UpdateProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var productMaped = mapper.Map<Product>(request.updateProductDto);
                    var newProduct =await unitOfWork.GetRepository<Product>().FirstOrDefaultAsync(p=>p.Id==request.updateProductDto.Id);
                    if (newProduct == null)
                    {
                        return await Task.FromResult(new UpdateProductDto { });
                    }
                    newProduct.Id= request.updateProductDto.Id;
                    newProduct.Name= request.updateProductDto.Name;
                    newProduct.ManufacturePhone = request.updateProductDto.ManufacturePhone;
                    newProduct.ManufactureEmail = request.updateProductDto.ManufactureEmail;
                    unitOfWork.GetRepository<Product>().Update(newProduct);
                    await unitOfWork.SaveChangesAsync();
                    return await Task.FromResult(request.updateProductDto);
                }
                catch (Exception e)
                {
                    Log.Error(e,e.ToString());
                    return await Task.FromException<UpdateProductDto>(e);
                }
               
            }
        }
    }
}
