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
                    var oldproduct = mapper.Map<Product>(request.updateProductDto);
                    unitOfWork.GetRepository<Product>().Update(oldproduct);
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
