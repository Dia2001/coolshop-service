using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;
using System.Security.Cryptography;
using System.Text;

namespace API_ShopingClose.Service
{
    public class UserDeptService
    {
        private readonly MySqlConnection _conn;

        public UserDeptService(MySqlConnection conn)
        {
            _conn = conn;
        }

        public IEnumerable<User> GetAllUser()
        {
            string getAllUsersCommand = "SELECT * FROM user;";
            var result = this._conn.Query<User>(getAllUsersCommand);
            return result;
        }

        public User Login(string username, string password)
        {
            string passwordlogin = GetMD5(password);

            string getUsersLogin = "SELECT * FROM user " +
                    "where Username='" + username + "' AND " +
                    "Password='" + passwordlogin + "';";

            var result = this._conn.Query<User>(getUsersLogin);
            var userlogin = result.FirstOrDefault();
            return userlogin;
        }

        public User GetProfileUser(string user_id)
        {
            string getUsersProfile = "SELECT * FROM user " +
                    "where UserID='" + user_id + "';";

            var result = this._conn.Query<User>(getUsersProfile);
            var userprofile = result.FirstOrDefault();
            return userprofile;
        }

        public Guid? InsertUser(User user)
        {
            bool b = false;
            string insertUserCommand = "INSERT INTO user (UserID, Username, Password, PhoneNumber, Address, Fullname, LastOperatingTime, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy, DeletedDate, RoleID)" +
                   "VALUES ( @UserID, @Username, @Password, @PhoneNumber, @Address, @Fullname, @LastOperatingTime, @CreatedDate, @CreatedBy, @ModifiedDate, @ModifiedBy, @DeletedDate, @RoleID);";

            var userID = Guid.NewGuid();
            var parameters = new DynamicParameters();
            parameters.Add("@UserID", userID);
            parameters.Add("@Username", user.Username);
            parameters.Add("@Password", GetMD5(user.PassWord));
            parameters.Add("@PhoneNumber", user.PhoneNumber);
            parameters.Add("@Address", user.Address);
            parameters.Add("@Fullname", user.FullName);
            parameters.Add("@LastOperatingTime", user.LastOperatingTime);
            parameters.Add("@CreatedDate", user.CreatedDate);
            parameters.Add("@CreatedBy", user.CreatedBy);
            parameters.Add("@ModifiedDate", user.ModifiedDate);
            parameters.Add("@ModifiedBy", user.ModifiedBy);
            parameters.Add("@DeletedDate", user.DeletedDate);
            parameters.Add("@RoleID", user.RoleID);
            b = this._conn.Execute(insertUserCommand, parameters) > 0;

            if (b)
            {
                return userID;
            }
            return null;
        }

        // Mã hóa mật khẩu
        public string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}
