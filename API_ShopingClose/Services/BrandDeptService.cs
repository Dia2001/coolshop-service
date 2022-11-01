using API_ShopingClose.Entities;
using Dapper;
using Microsoft.AspNetCore.Mvc;
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

        //lấy tất cả brand
        public IEnumerable<Brand> GetAllBrand()
        {
            string getAllBrandsCommand = "SELECT * FROM brand;";
            var result = this._conn.Query<Brand>(getAllBrandsCommand);
            return result;
        }

        // tạo mới brand
        public bool addBrand(Brand brand)
        {
            bool b = false;
            string sql = "INSERT INTO brand ( BrandName , Description)" +
                   "VALUES ( @BrandName , @Description);";

            var parameters = new DynamicParameters();
            parameters.Add("@BrandName", brand.BrandName);
            parameters.Add("@Description", brand.Description);
            b = this._conn.Execute(sql, parameters) > 0;

            return b;
        }

        // cập nhật brand
        public bool updateBrand(long brandId, Brand brand)
        {
            bool b = false;
            string sql = "Update brand set BrandName = @BrandName , Description = @Description" +
                                        " where BrandID = @BrandID";

            var parameters = new DynamicParameters();
            parameters.Add("@BrandID", brandId);
            parameters.Add("@BrandName", brand.BrandName);
            parameters.Add("@Description", brand.Description);
            b = this._conn.Execute(sql, parameters) > 0;

            return b;
        }

        internal object getBrandByID()
        {
            throw new NotImplementedException();
        }

        // xóa brand
        public bool deleteBrand(long brandId)
        {
            bool b = false;
            string sql = "Delete from brand where BrandID = @BrandID";

            var parameters = new DynamicParameters();
            parameters.Add("@BrandID", brandId);
            b = this._conn.Execute(sql, parameters) > 0;

            return b;
        }


    }
}
