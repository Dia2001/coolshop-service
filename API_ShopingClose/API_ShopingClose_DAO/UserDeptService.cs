using API_ShopingClose.Entities;
using API_ShopingClose.Helper;
using Dapper;
using MySqlConnector;
using System.Security.Cryptography;
using System.Text;

namespace API_ShopingClose.API_ShopingClose_DAO
{
    public class UserDeptService
    {
        private readonly MySqlConnection _conn;

        // private string _connectionString = AppSettings.Instance.ConnectionString;

        public UserDeptService(MySqlConnection conn)
        {
            _conn = conn;
            // _conn = new MySqlConnection(_connectionString);
        }

        public IEnumerable<User> GetAllUser()
        {
            string getAllUsersCommand = "SELECT * FROM users;";
            var result = this._conn.Query<User>(getAllUsersCommand);
            return result;
        }

        public User Login(string username, string password)
        {
            string passwordlogin = GetMD5(password);
            string getUsersLogin = "SELECT * FROM users " +
                    "where user_name='" + username + "' AND " +
                    "password='" + passwordlogin + "';";
            var result = this._conn.Query<User>(getUsersLogin);
            var userlogin = result.FirstOrDefault();
            return userlogin;
        }

        public User GetProfileUser(string user_id)
        {
            string getUsersProfile = "SELECT * FROM users " +
                    "where user_id='" + user_id + "';";

            var result = this._conn.Query<User>(getUsersProfile);
            var userprofile = result.FirstOrDefault();
            return userprofile;
        }

        public bool InsertUser(User user)
        {
            bool b = false;
            string insertUserCommand = "INSERT INTO users (user_id, user_name, password, phone_number, address, fullname, last_operating_time, created_date, created_by, modified_date, modified_by, deleted_date, role_id)" +
                   "VALUES ( @user_id, @user_name, @password, @phone_number, @address, @fullname, @last_operating_time, @created_date, @created_by, @modified_date, @modified_by, @deleted_date, @role_id);";

            var userID = Guid.NewGuid();
            var parameters = new DynamicParameters();
            parameters.Add("@user_id", userID);
            parameters.Add("@user_name", user.User_Name);
            parameters.Add("@password", GetMD5(user.PassWord));
            parameters.Add("@phone_number", user.Phone_Number);
            parameters.Add("@address", user.Address);
            parameters.Add("@fullname", user.FullName);
            parameters.Add("@last_operating_time", user.Last_Operating_Time);
            parameters.Add("@created_date", user.Created_Date);
            parameters.Add("@created_by", user.Created_By);
            parameters.Add("@modified_date", user.Modified_Date);
            parameters.Add("@modified_by", user.Modified_By);
            parameters.Add("@deleted_date", user.Deleted_Date);
            parameters.Add("@role_id", user.Role_ID);
            b = this._conn.Execute(insertUserCommand, parameters) > 0;

            return b;
        }

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
