using API_ShopingClose.Common;
using API_ShopingClose.Entities;
using API_ShopingClose.Models;
using API_ShopingClose.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CartsController : AuthController
    {
        private readonly IConfiguration _config;
        CartDeptService _cartservice;

        public CartsController(IConfiguration config, ILogger<UsersController> logger, 
            CartDeptService cartservice) : base(logger)
        {
            _config = config;
            _cartservice =cartservice;
        }

        [HttpPost]
        [Route("carts")]
        public async Task<IActionResult> CreateProductToCart([FromBody] CartModel cartmodel)
        {
            try
            {
                Cart cart = ConvertMethod.convertCartModelToCart(cartmodel);
                cart.userId = Guid.Parse(GetUserId().ToString());
                if (await _cartservice.InsertProductToCart(cart) == true)
                {
                    return StatusCode(StatusCodes.Status201Created,cart.productId);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (MySqlException mySqlException)
            {
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

    }
}
