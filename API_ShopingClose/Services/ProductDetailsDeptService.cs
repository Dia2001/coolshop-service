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

    public async Task<IEnumerable<ProductDetails>> getAllProductDetailByProductId(Guid productId)
    {
        string sql = "SELECT * from productdetails where ProductID = @ProductID";

        var parameters = new DynamicParameters();

        parameters.Add("@ProductID", productId);

        return await _conn.QueryAsync<ProductDetails>(sql, parameters);
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

    public async Task<bool> updateProductDetails(ProductDetails productDetails)
    {
        string sql = "UPDATE productdetails set ProductID = @ProductID, ColorID = @ColorID, SizeID = @SizeID, Quantity = @Quantity where ProductDetailsID = @ProductDetailsID";

        var parameters = new DynamicParameters();

        parameters.Add("@ProductDetailsID", productDetails.productDetailsId);
        parameters.Add("@ProductID", productDetails.productId);
        parameters.Add("@ColorID", productDetails.colorId);
        parameters.Add("@SizeID", productDetails.sizeId);
        parameters.Add("@Quantity", productDetails.quantity);

        return await _conn.ExecuteAsync(sql, parameters) > 0;
    }

    public async Task<bool> updateQuantityByProductIdAndSizeIdAndColorId(
        Guid productId, string sizeId, string colorId, int quantity)
    {
        string sql = "UPDATE productdetails set quantity = quantity + @quantity where ProductID = @ProductID and ColorID = @ColorID and SizeID = @SizeID";

        var parameters = new DynamicParameters();

        parameters.Add("@quantity", quantity);
        parameters.Add("@ProductID", productId);
        parameters.Add("@ColorID", colorId);
        parameters.Add("@SizeID", sizeId);

        return await _conn.ExecuteAsync(sql, parameters) > 0;
    }

    public async Task<bool> deleteAllProductDetailByProductIdAndSizeId(Guid productId, string sizeId)
    {
        string sql = "DELETE from productdetails where ProductID = @ProductID and SizeID = @SizeID";

        var parameters = new DynamicParameters();

        parameters.Add("@ProductID", productId);
        parameters.Add("@SizeID", sizeId);

        return await _conn.ExecuteAsync(sql, parameters) > 0;
    }

    public async Task<bool> deleteAllProductDetailByProductIdAndColorId(Guid productId, string colorId)
    {
        string sql = "DELETE from productdetails where ProductID = @ProductID and ColorID = @ColorID";

        var parameters = new DynamicParameters();

        parameters.Add("@ProductID", productId);
        parameters.Add("@ColorID", colorId);

        return await _conn.ExecuteAsync(sql, parameters) > 0;
    }
}
