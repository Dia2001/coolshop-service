using API_ShopingClose.Service;
using Microsoft.AspNetCore.Mvc;
using API_ShopingClose.Entities;
using API_ShopingClose.Model;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1/colors")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        ColorDeptService _colorservice;

        public ColorsController(ColorDeptService colorservice)
        {
            _colorservice = colorservice;
        }

        [HttpGet]
        public IActionResult GetAllColors()
        {
            try
            {
                var colors = _colorservice.GetAllColor();

                List<ColorModel> colorModels = new List<ColorModel>();

                foreach (Color color in colors)
                {
                    colorModels.Add(new ColorModel(color));
                }

                if (colors != null)
                {
                    return StatusCode(StatusCodes.Status200OK, colorModels);
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
