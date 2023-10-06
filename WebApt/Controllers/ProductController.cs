using Microsoft.AspNetCore.Mvc;
using MediatR;
using WebApi.DTOs;
using Application.DTOs.Product;
using Serilog;
using System.Text;
using Application.Contracts.Queries.Products;
using Application.Contracts.Commands.Products;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            GetAllProductsCommand getAllProducts = new GetAllProductsCommand();
            var getProducts = await mediator.Send(getAllProducts);
            ResponseDto<List<GetAllProductsResponse>> Response = new ResponseDto<List<GetAllProductsResponse>>()
            {
                Success = true,
                Message = (getProducts.Count == 0 ? "محصولی برای نمایش وجود ندارد" : ""),
                Data = getProducts
            };
            return Ok(Response);
        }

        // GET api/<ProductController>/5
        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(Guid productId)
        {
            GetProductByIdQuery getCommand = new GetProductByIdQuery(productId);
            var getResult = await mediator.Send(getCommand);
            if (getResult is null)
            {
                return Ok(new ResponseDto<GetProductByIdDto>
                {
                    Success = false,
                    Message = "محصول مورد نظر یافت نشد",
                    Data = getResult
                });
            }
            return Ok(new ResponseDto<GetProductByIdDto>
            {
                Success = true,
                Data = getResult

            });
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddProductDto addProductDto)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.Values.SelectMany(e => e.Errors);
                StringBuilder combineErrors = new StringBuilder();
                foreach (var item in modelErrors)
                {
                    combineErrors.Append(item.ErrorMessage + "-");
                }
                Log.Warning("Add Product=>ModelState Has Some Errors:{@Errors}", combineErrors.ToString());
                return Ok(new ResponseDto
                {
                    Success = false,
                    Message = "لطفا اطلاعات خواسته شده را وارد کنید"
                });
            }
            AddProductCommand productQuery = new AddProductCommand(addProductDto);
            AddProductResultDto? addResult =await mediator.Send(productQuery);
            return Created($"/api/product/get/{addResult.Id}", new ResponseDto<AddProductResultDto>
            {
                Success = true,
                Message = "محصول با موفقیت ثبت شد",
                Data = addResult
            });
        }

        // PUT api/<ProductController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateProductDto updateProductDto)
        {
            if (ModelState.IsValid is false)
            {
                var allErrors = ModelState.Values.SelectMany(e => e.Errors);
                StringBuilder combineErrors = new StringBuilder();
                foreach (var item in allErrors)
                {
                    combineErrors.Append(item.ErrorMessage + "-");
                }
                Log.Warning("Update Product=>ModelState Has Some Errors:{@Errors}", combineErrors.ToString());
                return Ok(new ResponseDto
                {
                    Success = false,
                    Message = "ویرایش محصول با خطا مواجه شد،لطفا اطلاعات خواسته شده را کامل وارد کنید"
                });
            }
            UpdateProductCommand productCommand = new UpdateProductCommand(updateProductDto);
            var updateResult = await mediator.Send(productCommand);
            if (updateProductDto is null)
            {

                return Ok(new ResponseDto
                {
                    Success = false,
                    Message = "ویرایش محصول با خطا مواجه شد،لطفا با پشتیبانی تماس بگیرید"
                });
            }
            return Ok(new ResponseDto<UpdateProductDto>
            {
                Success = true,
                Message="ویرایش محصول با موفقیت انجام شد",
                Data = updateResult
            });
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            DeleteProductCommand deleteProduct = new DeleteProductCommand(id);
            var deleteResult =await mediator.Send(deleteProduct);
            if (deleteResult is false)
            {
                return Ok(new ResponseDto { 
                Success=false,
                Message="محصول مورد نظر یافت نشد"
                });
            }
            return Ok(new ResponseDto
            {
                Success = true,
                Message = "محصول مورد نظر حذف شد"
            });
        }
    }
}
