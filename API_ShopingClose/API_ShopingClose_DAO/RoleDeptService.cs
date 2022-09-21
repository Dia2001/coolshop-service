using API_ShopingClose.Entities;
using API_ShopingClose.Helper;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.API_ShopingClose_DAO
{
    public class RoleDeptService
    {
        private readonly MySqlConnection _conn;

        private string _connectionString = AppSettings.Instance.ConnectionString;

        public RoleDeptService()
        {

            _conn = new MySqlConnection(_connectionString);
        }

        public IEnumerable<Role> GetAllRole()
        {
            string getAllRolesCommand = "SELECT * FROM roles;";
            var result = this._conn.Query<Role>(getAllRolesCommand);
            return result;
        }
    }
}
