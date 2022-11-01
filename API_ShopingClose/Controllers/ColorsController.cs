using API_ShopingClose.Service;
using Microsoft.AspNetCore.Mvc;
using API_ShopingClose.Entities;
using API_ShopingClose.Model;
using MySqlConnector;

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


        // tạo mới color
        [HttpPost]
        public IActionResult AddColor(Color color)
        {
            try
            {
                if (_colorservice.addColor(color) == true)
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



        // sửa Color
        [HttpPut]
        [Route("{colorId}")]
        public IActionResult UpdateColor([FromRoute] string colorId, [FromBody] Color color)
        {
            try
            {
                if (_colorservice.updateColor(colorId, color) == true)
                {
                    return StatusCode(StatusCodes.Status200OK, "Success");
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


        // xóa Color
        [HttpDelete]
        [Route("{colorId}")]
        public IActionResult DeleteColor([FromRoute] string colorId)
        {
            try
            {
                if (_colorservice.deleteColor(colorId) == true)
                {
                    return StatusCode(StatusCodes.Status200OK, "Success");
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
