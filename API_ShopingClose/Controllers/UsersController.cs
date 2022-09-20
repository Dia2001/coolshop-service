using API_ShopingClose.API_ShopingClose_DAO;
using API_ShopingClose.Entities;
using API_ShopingClose.Helper;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _config;
        UserDeptService _userservice;
        public UsersController( IConfiguration config)
        {
            _config = config;
            _userservice = new UserDeptService();
        }
        /// <summary>
        /// API lấy tất cả các user
        /// </summary>
        /// <returns>Danh sách tất cà các user</returns
        /// Created by: NVDIA(18/9/2022)
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<User>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllUsers()
        {
            try
            {    
                var users = _userservice.GetAllUser();

                // Nếu users khác null thì trả về toàn bộ user ngoài ra báo lỗi
                if (users != null)
                {
                    return StatusCode(StatusCodes.Status200OK, users);
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
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromQuery] string username, [FromQuery] string password)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            try
            {

                var users = _userservice.Login(username, password);

                // Nếu user khác null thì trả về token đăng nhập ngoài ra báo lỗi
                if (users != null)
                {
                    var token = GenerateToken(username, password);
                    return StatusCode(StatusCodes.Status200OK, token);
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

        // Trả về token đăng nhập
        private string GenerateToken(string username, string password)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
