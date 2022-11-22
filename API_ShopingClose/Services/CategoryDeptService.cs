using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.Service
{
    public class CategoryDeptService
    {
        private readonly MySqlConnection _conn;

        public CategoryDeptService(MySqlConnection conn)
        {

            _conn = conn;
        }

        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            string getAllCategoriesCommand = "SELECT * FROM category;";
            var result =await this._conn.QueryAsync<Category>(getAllCategoriesCommand);
            return result;
        }

        // tạo  category
        public bool addCategory(Category category)
        {
            bool b = false;
            string sql = "INSERT INTO category ( CategoryID , CategoryName , Description , Slug , IsShow)" +
                   "VALUES ( @CategoryID , @CategoryName , @Description , @Slug , @IsShow);";

            var parameters = new DynamicParameters();
            parameters.Add("@CategoryID", category.CategoryID);
            parameters.Add("@CategoryName", category.CategoryName);
            parameters.Add("@Description", category.Description);
            parameters.Add("@Slug", category.slug);
            parameters.Add("@IsShow", category.isShow);
            b = this._conn.Execute(sql, parameters) > 0;

            return b;
        }

        // cập nhật category
        public bool updateCategory(Category category)
        {
            bool b = false;
            string sql = "Update category set CategoryName = @CategoryName , Description = @Description ," +
                            " Slug = @Slug , IsShow = @IsShow where CategoryID = @CategoryID";

            var parameters = new DynamicParameters();
            parameters.Add("@CategoryID", category.CategoryID);
            parameters.Add("@CategoryName", category.CategoryName);
            parameters.Add("@Description", category.Description);
            parameters.Add("@Slug", category.slug);
            parameters.Add("@IsShow", category.isShow);
            b = this._conn.Execute(sql, parameters) > 0;

            return b;
        }

        // xóa category
        public bool deleteCategory(Category category)
        {
            bool b = false;
            string sql = "Delete from category where CategoryID = @CategoryID";

            var parameters = new DynamicParameters();
            parameters.Add("@CategoryID", category.CategoryID);
            b = this._conn.Execute(sql, parameters) > 0;

            return b;
        }

    }
}
