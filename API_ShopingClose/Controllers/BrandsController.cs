using API_ShopingClose.Service;
using Microsoft.AspNetCore.Mvc;
using API_ShopingClose.Model;
using API_ShopingClose.Entities;

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
    }
}
