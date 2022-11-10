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
        ProductDetailsDeptService _productDetailsDeptService;
        public CartsController(IConfiguration config, ILogger<UsersController> logger,
            CartDeptService cartservice, ProductDeptService productservice, ProductDetailsDeptService productDetailsDeptService) : base(logger)
        {
            _config = config;
            _cartservice = cartservice;
            _productservice = productservice;
            _productDetailsDeptService = productDetailsDeptService;
        }

        [HttpPost]
        [Route("carts")]
        public async Task<IActionResult> CreateProductToCart([FromBody] CartModel cartmodel)
        {
            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            try
            {
                Cart cart = ConvertMethod.convertCartModelToCart(cartmodel);
                cart.userId = Guid.Parse(GetUserId().ToString());

                Product product = (await _productservice.getOneProduct(cart.productId.ToString()));
                cart.productName = product.ProductName;
                cart.productImage = product.Image;
                cart.price = product.Price;
                Cart cartDb = await _cartservice.checkUserProductCart(cart.userId, cart.productId, cart.sizeId, cart.colorId);
                ProductDetails productDetail = await _productDetailsDeptService.getOneProductDetail(cart.productId, cart.sizeId, cart.colorId);
                if (cartDb != null && productDetail != null)
                {
                    cart.quantity += cartDb.quantity;

                    if (cart.quantity > 0)
                    {
                        if (cart.quantity <= productDetail.quantity)
                        {
                            if (await _cartservice.updateProductToCart(cart))
                            {
                                response = new
                                {
                                    status = 201,
                                    message = "Thêm sản phẩm vào giỏ hàng thành công"
                                };
                            }
                            else
                            {
                                throw new Exception("Thêm sản phẩm vào giỏ hàng thất bại.");
                            }
                            return StatusCode(StatusCodes.Status201Created, cart.productId);
                        }
                        else
                        {
                            throw new Exception("Số lượng sản phẩm không hợp lệ");
                        }
                    }
                    else
                    {
                        if (await _cartservice.DeleteCart(cart.userId, cart.productId, cart.sizeId, cart.colorId))
                        {
                            response = new
                            {
                                status = 200,
                                message = "Sản phẩm đã được xóa khỏi giỏ hàng!",
                            };
                        }
                        else
                        {
                            throw new Exception("Xóa sản phẩm khỏi giỏ hàng, thất bại");
                        }
                        return StatusCode(StatusCodes.Status200OK, response);
                    }
                }
                else
                {
                    if (cart.quantity > 0 && cart.quantity <= productDetail.quantity)
                    {
                        if (await _cartservice.InsertProductToCart(cart))
                        {
                            response = new
                            {
                                status = 201,
                                message = "Thêm giỏ hàng thành công!",
                            };
                        }
                        else
                        {
                            throw new Exception("Thêm sản phẩm vào giỏ hàng thất bại");
                        }
                        return StatusCode(StatusCodes.Status201Created, response);
                    }
                    else
                    {
                        throw new Exception("Số lượng sản phẩm không hợp lệ");
                    }

                }

            }
            catch (Exception exception)
            {
                response = new
                {
                    status = 201,
                    message = exception.Message,
                };
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, response);
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
