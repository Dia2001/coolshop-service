﻿using API_ShopingClose.Service;
using API_ShopingClose.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_ShopingClose.Model;
using API_ShopingClose.Helper;
using API_ShopingClose.Common;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UsersController : AuthController
    {
        private readonly IConfiguration _config;
        UserDeptService _userservice;

        public UsersController(IConfiguration config, ILogger<UsersController> logger,
            UserDeptService userDeptService) : base(logger)
        {
            _config = config;
            _userservice = userDeptService;
        }

        /// <summary>
        /// API lấy tất cả các user
        /// </summary>
        /// <returns>Danh sách tất cà các user</returns
        /// Created by: NVDIA(18/9/2022)
        [HttpGet]
        [Route("users")]
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

        /// <summary>
        /// API đăng nhâp
        /// </summary>
        /// <returns>Token xác thực đăng nhập thành công</returns
        /// Created by: NVDIA(20/9/2022)
        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromBody] UserLoginModel userLogin)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            try
            {
                var users = _userservice.Login(userLogin.username, userLogin.password);

                // Nếu user khác null thì trả về token đăng nhập ngoài ra báo lỗi
                if (users != null)
                {
                    var token = GenerateToken(users);
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

        /// <summary>
        /// API đăng ký
        /// </summary>
        /// <returns>ID user đươc đăng ký thành công</returns>
        /// Created by: NVDIA(20/9/2022)
        [HttpPost]
        [Route("users")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult Register([FromBody] UserRegisterModel userModel)
        {
            try
            {
                User user = new User();
                user.FullName = userModel.fullName;
                user.Username = userModel.userName;
                user.PhoneNumber = userModel.phoneNumber;
                user.PassWord = userModel.password;
                user.RoleID = Constants.ROLE_CUSTOMER;
                user.CreatedDate = DateTime.Now;
                user.DeletedDate = null;
                user.ModifiedDate = null;
                user.LastOperatingTime = DateTime.Now;

                Guid? userId = _userservice.InsertUser(user);
                //Nếu thêm thành công thì trả về id của user ngoài ra thì thông báo lỗi
                if (userId != null)
                {
                    return StatusCode(StatusCodes.Status201Created, userId);
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

        /// <summary>
        /// API lấy profile user đang đăng nhập
        /// </summary>
        /// <returns>User đang đăng nhập vào hệ thống</returns>
        /// Created by: NVDIA(20/9/2022)
        [Authorize]
        [HttpGet("profile")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(User))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetProfileUser()
        {
            try
            {
                var Id = GetUserId().ToString();

                // Nếu không tìm thấy Id thì trả về lỗi
                if (Id == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
                else
                {
                    var users = _userservice.GetProfileUser(Id);

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
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        // Trả về token đăng nhập
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var claims = new[]
            {
             new Claim(ClaimTypes.NameIdentifier,user.UserID.ToString()),
             new Claim(ClaimTypes.Name,user.Username),
             new Claim(ClaimTypes.Role,user.RoleID.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(double.Parse(AppSettings.Instance.ConnectionString)),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
