using Application.Contracts.Commands.Products;
using Application.DTOs.Product;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.Commands.Product
{
    public class AddProductTest
    {
        [Fact]
        public async void AddProduct_Should_Be_Return_Product()
        {
            //Arrange
            var mediatRMoq = new Mock<IMediator>();
            var guid = Guid.Parse("3bc1c524-1287-414c-b378-78d80d3c3e4e");
            var userDto = new IdentityUser
            {
                Id= "206FC853-87C6-402E-A07F-C26C3F4E895D",
                Email="Sina@gmail.com",
                UserName="Sina"
            };

            var product = new AddProductDto
            {
                IsAvailable = true,
                ManufactureEmail = "test@gmail.com",
                ManufacturePhone = "09358881758",
                Name = "Test"
            };

            //Act
            AddProductCommand addProduct = new AddProductCommand(product,userDto);
            var addResult = await mediatRMoq.Object.Send(addProduct);

            addResult = new AddProductResultDto
            {
                Id=Guid.Parse("8E6D4E34-40F1-4AFC-9BDD-CFA301803D45"),
                IsAvailable = true,
                ManufactureEmail = "test@gmail.com",
                ManufacturePhone = "09358881758",
                Name = "Test",
                ProductDate = DateTime.Now,
            };

            //Assert
            Assert.NotNull(addResult);
            Assert.IsAssignableFrom<AddProductResultDto>(addResult);

        }
    }
}
