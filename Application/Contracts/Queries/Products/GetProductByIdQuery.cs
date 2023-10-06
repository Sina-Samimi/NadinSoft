using Application.DTOs.Product;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Queries.Products
{
    public class GetProductByIdQuery : IRequest<GetProductByIdDto>
    {
        private readonly Guid productId;

        public GetProductByIdQuery(Guid productId)
        {
            this.productId = productId;
        }
        public class GetProductCommandHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdDto>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper mapper;

            public GetProductCommandHandler(IUnitOfWork unitOfWork
                , IMapper mapper)
            {
                this.unitOfWork = unitOfWork;
                this.mapper = mapper;
            }
            public async Task<GetProductByIdDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {
                var product = await unitOfWork.GetRepository<Product>().GetEntityAsync(p => p.Id == request.productId);
                var mappedProduct = mapper.Map<GetProductByIdDto>(product);
                return mappedProduct;
            }
        }
    }
}
