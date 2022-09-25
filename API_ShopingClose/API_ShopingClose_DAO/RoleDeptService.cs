using API_ShopingClose.Entities;
using API_ShopingClose.Helper;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.API_ShopingClose_DAO
{
    public class RoleDeptService
    {
        private readonly MySqlConnection _conn;


        public RoleDeptService(MySqlConnection conn)
        {

            _conn = conn;
        }

        public IEnumerable<Role> GetAllRole()
        {
            string getAllRolesCommand = "SELECT * FROM role;";
            var result = this._conn.Query<Role>(getAllRolesCommand);
            return result;
        }
    }
}
