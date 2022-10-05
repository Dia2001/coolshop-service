using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.Service
{
    public class BrandDeptService
    {
        private readonly MySqlConnection _conn;

        public BrandDeptService(MySqlConnection conn)
        {

            _conn = conn;
        }

        public IEnumerable<Brand> GetAllBrand()
        {
            string getAllBrandsCommand = "SELECT * FROM brand;";
            var result = this._conn.Query<Brand>(getAllBrandsCommand);
            return result;
        }

    }
}
