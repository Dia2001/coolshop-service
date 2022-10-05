using API_ShopingClose.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategorysController : ControllerBase
    {
        CategoryDeptService _categoryservice;

        public CategorysController(CategoryDeptService categoryservice)
        {
            _categoryservice = categoryservice;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            try
            {

                var categories = _categoryservice.GetAllCategory();

                if (categories != null)
                {
                    return StatusCode(StatusCodes.Status200OK, categories);
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
