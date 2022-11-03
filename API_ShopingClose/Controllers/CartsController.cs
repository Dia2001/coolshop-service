using API_ShopingClose.Common;
using API_ShopingClose.Entities;
using API_ShopingClose.Models;
using API_ShopingClose.Service;
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
        ProductDeptService _productservice;
        public CartsController(IConfiguration config, ILogger<UsersController> logger,
            CartDeptService cartservice, ProductDeptService productservice) : base(logger)
        {
            _config = config;
            _cartservice = cartservice;
            _productservice = productservice;
        }

        [HttpPost]
        [Route("carts")]
        public async Task<IActionResult> CreateProductToCart([FromBody] CartModel cartmodel)
        {
            try
            {
                Cart cart = ConvertMethod.convertCartModelToCart(cartmodel);
                cart.userId = Guid.Parse(GetUserId().ToString());

                Product product = (await _productservice.getOneProduct(cart.productId.ToString()));
                cart.productName = product.ProductName;
                cart.productImage = product.Image;
                cart.price = product.Price;
                Cart cartDb = await _cartservice.checkUserProductCart(cart.userId, cart.productId, cart.sizeId, cart.colorId);

                if (cartDb != null)
                {
                    cart.quantity += cartDb.quantity;
                    if (await _cartservice.updateProductToCart(cart))
                    {
                        return StatusCode(StatusCodes.Status201Created, cart.productId);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "e002");
                    }
                }
                else
                {
                    if (await _cartservice.InsertProductToCart(cart) == true)
                    {
                        return StatusCode(StatusCodes.Status201Created, cart.productId);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "e002");
                    }
                }

            }
            catch (MySqlException mySqlException)
            {
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                Console.WriteLine(mySqlException.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        [HttpGet]
        [Route("carts/products")]
        public async Task<IActionResult> getCartByUserID()
        {
            try
            {
                Guid userId = Guid.Parse(GetUserId().ToString());
                List<Cart> carts = (await _cartservice.GetAllCartByUserId(userId)).ToList();
                return StatusCode(StatusCodes.Status200OK, carts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dynamic response = new
                {
                    status = 500,
                    message = "Call servser faile!",
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
