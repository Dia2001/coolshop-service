using API_ShopingClose.Service;
using Microsoft.AspNetCore.Mvc;
using API_ShopingClose.Entities;
using API_ShopingClose.Model;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1/sizes")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        SizeDeptService _sizeservice;

        public SizesController(SizeDeptService sizeservice)
        {
            _sizeservice = sizeservice;
        }

        [HttpGet]
        public IActionResult GetAllSizes()
        {
            try
            {
                var sizes = _sizeservice.GetAllSize();

                List<SizeModel> sizeModels = new List<SizeModel>();

                foreach (Size size in sizes)
                {
                    sizeModels.Add(new SizeModel(size));
                }

                if (sizes != null)
                {
                    return StatusCode(StatusCodes.Status200OK, sizeModels);
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
