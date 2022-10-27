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


        // tạo  Color
        public bool addColor(Color color)
        {
            bool b = false;
            string sql = "INSERT INTO color ( ColorID , ColorName , Description)" +
                   "VALUES ( @ColorID , @ColorName , @Description);";

            var parameters = new DynamicParameters();
            parameters.Add("@ColorID", color.ColorID);
            parameters.Add("@ColorName", color.ColorName);
            parameters.Add("@Description", color.Description);
            b = this._conn.Execute(sql, parameters) > 0;

            return b;
        }

        // cập nhật color
        public bool updateColor(Color color)
        {
            bool b = false;
            string sql = "Update color set ColorName = @ColorName , Description = @Description" +
                                        " where ColorID = @ColorID";

            var parameters = new DynamicParameters();
            parameters.Add("@ColorID", color.ColorID);
            parameters.Add("@ColorName", color.ColorName);
            parameters.Add("@Description", color.Description);
            b = this._conn.Execute(sql, parameters) > 0;

            return b;
        }

        // xóa color
        public bool deleteColor(Color color)
        {
            bool b = false;
            string sql = "Delete from color where ColorID = @ColorID";

            var parameters = new DynamicParameters();
            parameters.Add("@ColorID", color.ColorID);
            b = this._conn.Execute(sql, parameters) > 0;

            return b;
        }
    }
}
