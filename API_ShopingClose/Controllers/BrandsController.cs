using API_ShopingClose.Service;
using Microsoft.AspNetCore.Mvc;
using API_ShopingClose.Model;
using API_ShopingClose.Entities;
using Swashbuckle.AspNetCore.Annotations;
using MySqlConnector;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1/brands")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        BrandDeptService _brandservice;

        public BrandsController(BrandDeptService brandservice)
        {
            _brandservice = brandservice;
        }

        [HttpGet]
        public IActionResult GetAllBrands()
        {
            try
            {

                var brands = _brandservice.GetAllBrand();

                List<BrandModel> brandModels = new List<BrandModel>();

                foreach (Brand brand in brands)
                {
                    brandModels.Add(new BrandModel(brand));
                }

                if (brands != null)
                {
                    return StatusCode(StatusCodes.Status200OK, brandModels);
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


        // tạo mới brand
        [HttpPost]
        public IActionResult AddBrand(Brand brand)
        {
            try
            {
                if (_brandservice.addBrand(brand) == true)
                {
                    return StatusCode(StatusCodes.Status201Created, "Success");
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


        // sửa brand
        [HttpPut]
        public IActionResult UpdateBrand([FromBody] Brand brand)
        {
            try
            {
                if (_brandservice.updateBrand(brand) == true)
                {
                    return StatusCode(StatusCodes.Status201Created, "Success");
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


        // xóa brand
        [HttpDelete]
        public IActionResult DeleteBrand([FromBody] Brand brand)
        {
            try
            {
                if (_brandservice.deleteBrand(brand) == true)
                {
                    return StatusCode(StatusCodes.Status201Created, "Success");
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

        private IActionResult StatusCode(int status201Created, Func<OkResult> ok)
        {
            throw new NotImplementedException();
        }
    }
}
