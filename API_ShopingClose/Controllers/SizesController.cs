using API_ShopingClose.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

                if (sizes != null)
                {
                    return StatusCode(StatusCodes.Status200OK,sizes);
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
