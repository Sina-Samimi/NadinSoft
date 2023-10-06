using Application.DTOs.Product;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;
using Persistance.ConcractsImplementation;
using Persistance.Contracts;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Queries.Products
{
    public class GetAllProductsCommand : IRequest<List<GetAllProductsResponse>>
    {


    }
    public class GetAllProductsHanlder : IRequestHandler<GetAllProductsCommand, List<GetAllProductsResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetAllProductsHanlder(IUnitOfWork unitOfWork
            , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        Task<List<GetAllProductsResponse>> IRequestHandler<GetAllProductsCommand, List<GetAllProductsResponse>>.Handle(GetAllProductsCommand request, CancellationToken cancellationToken)
        {
            var allProducts = unitOfWork.GetRepository<Product>().GetAllAsync().Result;
            var mappProducts = mapper.Map<List<GetAllProductsResponse>>(allProducts);
            return Task.FromResult(mappProducts);
        }

    }

}
