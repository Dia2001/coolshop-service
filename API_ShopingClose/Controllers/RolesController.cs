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
        private readonly IConfiguration _config;
        private string connectionString = AppSettings.Instance.ConnectionString;

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
                // Khởi tạo kết nối tới DB MySQL
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh truy vấn
                string getAllRolesCommand = "SELECT * FROM roles;";

                // Thực hiện gọi vào DB để chạy câu lệnh truy vấn ở trên
                var roles = mySqlConnection.Query<Role>(getAllRolesCommand);

                // Xử lý dữ liệu trả về
                if (roles != null)
                {
                    // Trả về dữ liệu cho client
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
