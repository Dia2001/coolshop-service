using API_ShopingClose.Entities;
using API_ShopingClose.Helper;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.API_ShopingClose_DAO
{
    public class UserDeptService
    {
        private readonly MySqlConnection _conn;

        private string _connectionString = AppSettings.Instance.ConnectionString;
        public UserDeptService()
        {

            _conn = new MySqlConnection(_connectionString);
        }
        public IEnumerable<User> GetAllUser()
        {
            string getAllUsersCommand = "SELECT * FROM users;";
            var result = this._conn.Query<User>(getAllUsersCommand);
            return result;
        }
        public IEnumerable<User> Login(string username,string password)
        {
            string getUsersLogin= "SELECT * FROM users " +
                    "where user_name='" + username + "' AND " +
                    "password='" + password + "';"; 
            var result = this._conn.Query<User>(getUsersLogin);
            return result;
        }
    }
}
