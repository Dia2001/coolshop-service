using API_ShopingClose.API_ShopingClose_DAO;
using API_ShopingClose.Entities;
using API_ShopingClose.Helper;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        RoleDeptService _roleservice;
        public RolesController()
        {
            _roleservice =new RoleDeptService();
        }
        /// <summary>
        /// API lấy tất cả các role
        /// </summary>
        /// <returns>Danh sách tất cà các role</returns
        /// Created by: NVDIA(18/9/2022)
        [HttpGet]
        public IActionResult GetAllRoles()
        {
            try
            {

                var roles=_roleservice.GetAllRole();
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
