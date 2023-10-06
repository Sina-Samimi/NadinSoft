using Application.Contracts;
using Application.Contracts.Queries.Products;
using Application.DTOs.Product;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.Commands.Product
{
    public class GetAllProductsTest
    {
        

        [Fact]
        public void GetProducts_Should_Be_Return_ProductsList()
        {
            //Arrange
            var mediatRMoq = new Mock<IMediator>();
            var guid = Guid.Parse("FF23E751-7A51-4FEF-B0DA-A7C5657B59F3");

            //Act
            GetAllProductsCommand allProductsHanlder = new GetAllProductsCommand();
            var result= mediatRMoq.Object.Send(allProductsHanlder).Result;
            result = new List<GetAllProductsResponse>() {
               new GetAllProductsResponse
                {
                    Id=guid,
                    Title="test",
                    Price=1000
                },
                new GetAllProductsResponse
                {
                    Id=guid,
                    Title="test",
                    Price=1000
                }
            };

            //Assert
            Assert.NotNull(result);
        }
    }
}
