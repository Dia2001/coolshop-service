using API_ShopingClose.Service;
using API_ShopingClose.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        RoleDeptService _roleservice;

        public RolesController(RoleDeptService roleservice)
        {
            _roleservice = roleservice;
        }

        /// <summary>
        /// API lấy tất cả các role
        /// </summary>
        /// <returns>Danh sách tất cà các role</returns
        /// Created by: NVDIA(18/9/2022)
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Role))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllRoles()
        {
            try
            {

                var roles = _roleservice.GetAllRole();
                // Nếu roles khác null thì trả về toàn bộ các role ngoài ra thì báo lỗi
                if (roles != null)
                {
                    return StatusCode(StatusCodes.Status200OK, roles);
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
