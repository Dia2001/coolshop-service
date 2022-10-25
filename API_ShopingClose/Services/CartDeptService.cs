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

        public async Task<bool> updateProductToCart(Cart cart)
        {
            bool b = false;
            string sql = "UPDATE cart " +
                                    "SET Quantity =@Quantity" +
                                    "WHERE UserID=@UserID AND ProductID =@ProductID;";

            var parameters = new DynamicParameters();
            parameters.Add("@Quantity",cart.quantity);
            parameters.Add("@UserID",cart.userId);
            parameters.Add("@ProductID",cart.productId);

            return await this._conn.ExecuteAsync(sql, parameters) > 0;       
        
        }
        public async Task<Cart> checkUserProductCart(Guid userId, Guid productId)
        {
            bool b = false;
            string sql = "SELECT * FROM cart where UserID=@UserID AND ProductID = @ProductID;";

            var parameters = new DynamicParameters();
            parameters.Add("@UserID", userId);
            parameters.Add("@ProductID",productId);
            Cart result = this._conn.ExecuteScalar<Cart>(sql, parameters);
            return result;
        }
    }
}
