using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.Service;

public class ProductInCategoryDeptService
{
    private readonly MySqlConnection _conn;

    public ProductInCategoryDeptService(MySqlConnection conn)
    {
        _conn = conn;
    }

    public async Task<bool> InsertPrductInCategory(ProductInCategory productInCategory)
    {
        string sql = "INSERT INTO productincategory(ProductInCategoryID, ProductID, CategoryID)"
          + "VALUES (@ProductInCategoryID, @ProductID, @CategoryID)";

        var parameters = new DynamicParameters();
        Guid productInCategoryID = Guid.NewGuid();

        parameters.Add("@ProductInCategoryID", productInCategoryID);
        parameters.Add("@ProductID", productInCategory.productId);
        parameters.Add("@CategoryID", productInCategory.categoryId);

        return await _conn.ExecuteAsync(sql, parameters) > 0;
    }
}
