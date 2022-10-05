using API_ShopingClose.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1/colors")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        ColorDeptService _colorservice;

        public ColorsController(ColorDeptService colorservice)
        {
            _colorservice =colorservice;
        }

        [HttpGet]
        public IActionResult GetAllColors()
        {
            try
            {

                var colors = _colorservice.GetAllColor();

                if (colors != null)
                {
                    return StatusCode(StatusCodes.Status200OK, colors);
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
