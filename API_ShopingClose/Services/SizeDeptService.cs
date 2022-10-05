using Dapper;
using MySqlConnector;
using System.Drawing;

namespace API_ShopingClose.Service
{
    public class SizeDeptService
    {
        private readonly MySqlConnection _conn;

        public SizeDeptService(MySqlConnection conn)
        {

            _conn = conn;
        }

        public IEnumerable<Size> GetAllSize()
        {
            string getAllSizesCommand = "SELECT * FROM size;";
            var result = this._conn.Query<Size>(getAllSizesCommand);
            return result;
        }
    }
}
