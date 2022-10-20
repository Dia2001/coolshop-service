using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.Services
{
    public class CartDeptService
    {
        private readonly MySqlConnection _conn;

        public CartDeptService(MySqlConnection conn)
        {

            _conn = conn;
        }
        public async Task<bool> InsertProductToCart(Cart cart)
        {
            bool b = false;
            string insertCartCommand = "INSERT INTO cart (UserID, ProductID, SizeID, ColorID, ProductName, ProductImage, Quantity)" +
                   "VALUES (@UserID,@ProductID,@SizeID,@ColorID,@ProductName,@ProductImage,@Quantity);";

            var parameters = new DynamicParameters();
            parameters.Add("@UserID",cart.userId);
            parameters.Add("@ProductID",cart.productId);
            parameters.Add("@SizeID",cart.sizeId);
            parameters.Add("@ColorID",cart.colorId);
            parameters.Add("@ProductName",cart.productName);
            parameters.Add("@ProductImage",cart.productImage);
            parameters.Add("@Quantity",cart.quantity);

            return await this._conn.ExecuteAsync(insertCartCommand, parameters) > 0;
        }

    }
}
