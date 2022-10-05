using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.Service
{
    public class ColorDeptService
    {
        private readonly MySqlConnection _conn;

        public ColorDeptService(MySqlConnection conn)
        {

            _conn = conn;
        }

        public IEnumerable<Color> GetAllColor()
        {
            string getAllColorsCommand = "SELECT * FROM color;";
            var result = this._conn.Query<Color>(getAllColorsCommand);
            return result;
        }
    }
}
