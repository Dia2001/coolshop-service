using API_ShopingClose.Service;
using Microsoft.AspNetCore.Mvc;
using API_ShopingClose.Entities;
using API_ShopingClose.Model;

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

                List<CategoryModel> categoryModels = new List<CategoryModel>();

                foreach (Category category in categories)
                {
                    categoryModels.Add(new CategoryModel(category));
                }

                if (categories != null)
                {
                    return StatusCode(StatusCodes.Status200OK, categoryModels);
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
