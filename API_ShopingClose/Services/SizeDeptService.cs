using Dapper;
using MySqlConnector;
using API_ShopingClose.Entities;

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


        // tạo  Size
        public bool addSize(Size size)
        {
            bool b = false;
            string sql = "INSERT INTO size ( SizeID , SizeName , Description)" +
                   "VALUES ( @SizeID , @SizeName , @Description);";

            var parameters = new DynamicParameters();
            parameters.Add("@SizeID", size.SizeID);
            parameters.Add("@SizeName", size.SizeName);
            parameters.Add("@Description", size.Description);
            b = this._conn.Execute(sql, parameters) > 0;

            return b;
        }

        // cập nhật size
        public bool updateSize(string sizeId, Size size)
        {
            bool b = false;
            string sql = "Update size set SizeName = @SizeName , Description = @Description" +
                                        " where SizeID = @SizeID";

            var parameters = new DynamicParameters();
            parameters.Add("@SizeID", sizeId);
            parameters.Add("@SizeName", size.SizeName);
            parameters.Add("@Description", size.Description);
            b = this._conn.Execute(sql, parameters) > 0;

            return b;
        }

        // xóa size
        public bool deleteSize(string sizeId)
        {
            bool b = false;
            string sql = "Delete from size where SizeID = @SizeID";

            var parameters = new DynamicParameters();
            parameters.Add("@SizeID", sizeId);
            b = this._conn.Execute(sql, parameters) > 0;

            return b;
        }

    }
}
