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

        public IEnumerable<Category> GetAllCategory()
        {
            string getAllCategoriesCommand = "SELECT * FROM category;";
            var result = this._conn.Query<Category>(getAllCategoriesCommand);
            return result;
        }

    }
}
