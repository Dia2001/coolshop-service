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
        private string connectionString = AppSettings.Instance.ConnectionString;
        public UsersController( IConfiguration config)
        {
            _config = config;
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
        public IActionResult GetAllRoles()
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh truy vấn
                string getAllUsersCommand = "SELECT * FROM users;";

                // Thực hiện gọi vào DB để chạy câu lệnh truy vấn ở trên
                var users = mySqlConnection.Query<User>(getAllUsersCommand);

                // Xử lý dữ liệu trả về
                if (users != null)
                {
                    // Trả về dữ liệu cho client
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
                // Khởi tạo kết nối tới DB MySQL
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh truy vấn
                string getUsersByUPCommand = "SELECT * FROM users " +
                    "where user_name='" + username + "' AND " +
                    "password='" + password + "';";

                // Thực hiện gọi vào DB để chạy câu lệnh truy vấn ở trên
                var users = mySqlConnection.Query<User>(getUsersByUPCommand);

                // Xử lý dữ liệu trả về
                if (users != null)
                {
                    var token = GenerateToken(username, password);
                    // Trả về dữ liệu cho client
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
