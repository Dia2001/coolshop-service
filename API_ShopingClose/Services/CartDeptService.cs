using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.Service
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
            string insertCartCommand = "INSERT INTO cart (UserID, ProductID, SizeID, ColorID, ProductName, ProductImage, Quantity)" +
                   "VALUES (@UserID,@ProductID,@SizeID,@ColorID,@ProductName,@ProductImage,@Quantity, @Price);";

            var parameters = new DynamicParameters();
            parameters.Add("@UserID", cart.userId);
            parameters.Add("@ProductID", cart.productId);
            parameters.Add("@SizeID", cart.sizeId);
            parameters.Add("@ColorID", cart.colorId);
            parameters.Add("@ProductName", cart.productName);
            parameters.Add("@ProductImage", cart.productImage);
            parameters.Add("@Quantity", cart.quantity);
            parameters.Add("@Price", cart.price);

            return await this._conn.ExecuteAsync(insertCartCommand, parameters) > 0;
        }

        public async Task<bool> updateProductToCart(Cart cart)
        {
            string sql = "UPDATE cart " +
                                    "SET Quantity =@Quantity, Price = @Price" +
                                    " WHERE UserID=@UserID AND ProductID =@ProductID AND SizeID=@SizeID AND ColorID=@ColorID;";

            var parameters = new DynamicParameters();
            parameters.Add("@Quantity", cart.quantity);
            parameters.Add("@UserID", cart.userId);
            parameters.Add("@ProductID", cart.productId);
            parameters.Add("@SizeID", cart.sizeId);
            parameters.Add("@ColorID", cart.colorId);
            parameters.Add("@Price", cart.price);

            return await this._conn.ExecuteAsync(sql, parameters) > 0;
        }
        public async Task<Cart> checkUserProductCart(Guid userId, Guid productId, string sizeId, string colorId)
        {
            try
            {
                bool b = false;
                string sql = "SELECT * FROM cart where UserID=@UserID AND ProductID = @ProductID AND SizeID=@SizeID AND ColorID=@ColorID;";

                var parameters = new DynamicParameters();
                parameters.Add("@UserID", userId);
                parameters.Add("@ProductID", productId);
                parameters.Add("@SizeID", sizeId);
                parameters.Add("@ColorID", colorId);
                Cart result = await this._conn.QueryFirstAsync<Cart>(sql, parameters);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Cart>> GetAllCartByUserId(Guid userId)
        {
            string sql = "SELECT * FROM cart where UserID=@UserID;";
            var parameters = new DynamicParameters();
            parameters.Add("@UserID", userId);

            return await this._conn.QueryAsync<Cart>(sql, parameters);
        }
    }
}
