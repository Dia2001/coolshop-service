using API_ShopingClose.API_ShopingClose_DAO;
using API_ShopingClose.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;

namespace API_ShopingClose.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        ProductDeptService _productservice;

        public ProductsController(ProductDeptService productservice)
        {
            _productservice = productservice;
        }
        /// <summary>
        /// API lấy tất cả các sản phẩm
        /// </summary>
        /// <returns>Danh sách tất cà các sản phẩm</returns
        /// Created by: NVDIA(28/9/2022)
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<Product>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllUsers()
        {
            try
            {
                var products = _productservice.GetAllProduct();

                // Nếu products khác null thì trả về toàn bộ sản phẩm ngoài ra báo lỗi
                if (products != null)
                {
                    return StatusCode(StatusCodes.Status200OK, products);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// API thêm sản phẩm
        /// </summary>
        /// <returns>ID  sản phẩm đươc thêm thành công</returns>
        /// Created by: NVDIA(28/9/2022)
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult InsertProduct([FromBody]Product product)
        {
            try
            {
                //Nếu thêm thành công thì trả về id của product ngoài ra thì thông báo lỗi
                if (_productservice.InsertProduct(product) == true)
                {
                    return StatusCode(StatusCodes.Status201Created, product.ProductID);
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

        /// <summary>
        /// API sửa một sản phẩm
        /// </summary>
        /// <param name="product">Đối tượng sản phẩm cần sửa</param>
        /// <param name="productID">ID sản phẩm muốn sửa</param>
        /// <returns>ID sản phẩm cần sửa</returns>
        /// Create by: NVDIA (28/9/2022)
        [HttpPut]
        [Route("{productID}")]
        public IActionResult UpdateProduct([FromBody] Product product, [FromRoute] Guid productID)
        {
            try
            {

                //Nếu sửa thành công thì trả về id của product ngoài ra thì thông báo lỗi
                if (_productservice.UpdateProduct(product,productID) == true)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, productID);
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

        /// <summary>
        /// API xóa sản phẩm
        /// </summary>
        /// <param name="productID">ID của sản phẩm muốn xóa</param>
        /// <returns>ID sản phẩm cần xóa</returns>
        /// Create by: NVDIA (28/9/2022)
        [HttpDelete]
        [Route("{productID}")]
        public IActionResult DeleteProduct([FromRoute] Guid productID)
        {
            try
            {
                //Nếu xóa thành công thì trả về id của product ngoài ra thì thông báo lỗi
                if (_productservice.DeleteProduct(productID) == true)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK,productID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }
    }
}
