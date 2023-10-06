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
    public class DeleteProductCommand : IRequest<bool>
    {
        private readonly Guid productId;

        public DeleteProductCommand(Guid productId)
        {
            this.productId = productId;
        }
        public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper mapper;

            public DeleteProductCommandHandler(IUnitOfWork unitOfWork
                , IMapper mapper)
            {
                this.unitOfWork = unitOfWork;
                this.mapper = mapper;
            }
            public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {

                Product product = await unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(p => p.Id == request.productId);
                if (product is null)
                    return false;
                try
                {
                    unitOfWork.GetRepository<Product>().DeleteWithProperty(product);
                    await unitOfWork.SaveChangesAsync();
                    return true;
                }
                catch (Exception e)
                {
                    Log.Error(e,e.ToString());
                    return await Task.FromException<bool>(e);
                }
            
            }
        }
    }
}
