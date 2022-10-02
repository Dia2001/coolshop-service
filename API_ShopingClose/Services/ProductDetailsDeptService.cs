using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.Service;

public class ProductDetailsDeptService
{
    private readonly MySqlConnection _conn;

    public ProductDetailsDeptService(MySqlConnection conn)
    {
        _conn = conn;
    }

    public async Task<bool> InsertProductDetails(ProductDetails productDetails)
    {
        string sql = "INSERT INTO productdetails(ProductDetailsID, ProductID, ColorID, SizeID, Quantity)"
          + "VALUES (@ProductDetailsID, @ProductID, @ColorID, @SizeID, @Quantity)";

        var parameters = new DynamicParameters();
        Guid productDetailsID = Guid.NewGuid();

        parameters.Add("@ProductDetailsID", productDetailsID);
        parameters.Add("@ProductID", productDetails.productId);
        parameters.Add("@ColorID", productDetails.colorId);
        parameters.Add("@SizeID", productDetails.sizeId);
        parameters.Add("@Quantity", productDetails.quantity);

        return await _conn.ExecuteAsync(sql, parameters) > 0;
    }
}
