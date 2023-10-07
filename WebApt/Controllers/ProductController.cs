using Microsoft.AspNetCore.Mvc;
using MediatR;
using WebApi.DTOs;
using Application.DTOs.Product;
using Serilog;
using System.Text;
using Application.Contracts.Queries.Products;
using Application.Contracts.Commands.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Application.DTOs.User;
using System.Security.Claims;
using Application.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public ProductController(IMediator mediator
            , UserManager<IdentityUser> userManager
            , IAuthorizationService authorizationService)
        {
            this.mediator = mediator;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }
        // GET: api/<ProductController>
        [HttpGet]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid productId)
        {
            var getResult = GetProductById(productId).Result;
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
        /// <summary>
        /// </summary>
        /// <example email="sina@gmail.com"></example>
        /// <param name="addProductDto"></param>
        /// <returns></returns>

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
            string userName = User.Claims.Single(p => p.Type == "Name").Value;
            var user = await _userManager.FindByNameAsync(userName);
            AddProductCommand productQuery = new AddProductCommand(addProductDto,user);
            AddProductResultDto? addResult = await mediator.Send(productQuery);
            return Created($"/api/product/get/{addResult.Id}", new ResponseDto<AddProductResultDto>
            {
                Success = true,
                Message = "محصول با موفقیت ثبت شد",
                Data = addResult
            });
        }

        // PUT api/<ProductController>/5
        /// <summary>
        /// Edit Product
        /// </summary>
        /// <param name="updateProductDto">Product Data For Update</param>
        /// <returns>return Updatetd Product When is Successed</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Product Not Found</response>
        /// <response code="401">If You Dont Authorized Or Your Token Expired</response>
        /// <response code="500">Server Error Should Contact To Support Team</response>
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

                //Log.Warning("Update Product=>ModelState Has Some Errors:{@Errors}", combineErrors.ToString());
                return Ok(new ResponseDto
                {
                    Success = false,
                    Message = "ویرایش محصول با خطا مواجه شد،لطفا اطلاعات خواسته شده را کامل وارد کنید"
                });
            }

            var product =await GetProductById(updateProductDto.Id);
            var auth = new AuthorizationRquirmentDto<GetProductByIdDto>() { Dto=product};
            var result =_authorizationService.AuthorizeAsync(User, auth, "IsProductForUser").Result;

            if (result.Succeeded is false)
            {
                HttpContext.Response.StatusCode = 401;

                return Ok(new ResponseDto
                {
                    Success = false,
                    Message = "شما دسترسی ویرایش این محصول را ندارید"
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
                Message = "ویرایش محصول با موفقیت انجام شد",
                Data = updateResult
            });
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await GetProductById(id);
            var auth = new AuthorizationRquirmentDto<GetProductByIdDto>() { Dto = product };
            var result = _authorizationService.AuthorizeAsync(User, auth, "IsProductForUser").Result;

            if (result.Succeeded is false)
            {
                HttpContext.Response.StatusCode = 401;

                return Ok(new ResponseDto
                {
                    Success = false,
                    Message = "شما دسترسی حذف این محصول را ندارید"
                });
            }
            DeleteProductCommand deleteProduct = new DeleteProductCommand(id);
            var deleteResult = await mediator.Send(deleteProduct);
            if (deleteResult is false)
            {
                return Ok(new ResponseDto
                {
                    Success = false,
                    Message = "محصول مورد نظر یافت نشد"
                });
            }
            return Ok(new ResponseDto
            {
                Success = true,
                Message = "محصول مورد نظر حذف شد"
            });
        }

        private async Task<GetProductByIdDto> GetProductById(Guid productId)
        {
            GetProductByIdQuery getCommand = new GetProductByIdQuery(productId);
            var getResult = await mediator.Send(getCommand);
            return getResult;
        }
    }
}
