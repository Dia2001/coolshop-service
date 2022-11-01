using API_ShopingClose.Service;
using Microsoft.AspNetCore.Mvc;
using API_ShopingClose.Entities;
using API_ShopingClose.Model;
using MySqlConnector;

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


        // tạo mới category
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            try
            {
                if (_categoryservice.addCategory(category) == true)
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



        // sửa Category
        [HttpPut]
        public IActionResult UpdateCategory(Category category)
        {
            try
            {
                if (_categoryservice.updateCategory(category) == true)
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


        // xóa Category
        [HttpDelete]
        public IActionResult DeleteCategory(Category category)
        {
            try
            {
                if (_categoryservice.deleteCategory(category) == true)
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
