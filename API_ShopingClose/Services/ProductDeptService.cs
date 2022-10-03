using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.Service
{
    public class ProductDeptService
    {
        private readonly MySqlConnection _conn;

        public ProductDeptService(MySqlConnection conn)
        {

            _conn = conn;
        }

        public IEnumerable<Product> GetAllProduct()
        {
            string getAllProductsCommand = "SELECT * FROM product;";
            var result = this._conn.Query<Product>(getAllProductsCommand);
            return result;
        }

        public Guid? InsertProduct(Product product)
        {
            string insertProductCommand = "INSERT INTO product (ProductID, BrandID, ProductName, Price, Image, Rate, Slug, Description)" +
                   "VALUES (@ProductID,@BrandID,@ProductName,@Price,@Image,@Rate,@Slug,@Description);";

            var productID = Guid.NewGuid();
            var parameters = new DynamicParameters();
            parameters.Add("@ProductID", productID);
            parameters.Add("@BrandID", product.BrandID);
            parameters.Add("@ProductName", product.ProductName);
            parameters.Add("@Price", product.Price);
            parameters.Add("@Image", product.Image);
            parameters.Add("@Rate", product.Rate);
            parameters.Add("@Slug", product.Slug);
            parameters.Add("@Description", product.Description);
            if (this._conn.Execute(insertProductCommand, parameters) > 0)
            {
                return productID;
            }

            return null;
        }

        public async Task<bool> UpdateProduct(Product product, Guid productID)
        {
            bool b = false;
            string updateProductCommand = "UPDATE product " +
                                    "SET BrandID =@BrandID, " +
                                    "ProductName =@ProductName, " +
                                    "Price =@Price, " +
                                    "Image =@Image, " +
                                    "Rate =@Rate, " +
                                    "Slug =@Slug, " +
                                    "Description=@Description " +
                                    "WHERE ProductID =@ProductID;";

            var parameters = new DynamicParameters();
            parameters.Add("@ProductID", productID);
            parameters.Add("@BrandID", product.BrandID);
            parameters.Add("@ProductName", product.ProductName);
            parameters.Add("@Price", product.Price);
            parameters.Add("@Image", product.Image);
            parameters.Add("@Rate", product.Rate);
            parameters.Add("@Slug", product.Slug);
            parameters.Add("@Description", product.Description);
            b = await this._conn.ExecuteAsync(updateProductCommand, parameters) > 0;

            return b;
        }

        public bool DeleteProduct(Guid productID)
        {
            bool b = false;
            string deleteProductCommand = "DELETE FROM product WHERE ProductID =@ProductID";

            var parameters = new DynamicParameters();
            parameters.Add("@ProductID", productID);
            b = this._conn.Execute(deleteProductCommand, parameters) > 0;

            return b;
        }

        public async Task<bool> checkSlugEmpty(string slug)
        {
            string sql = "SELECT * FROM product WHERE Slug = @Slug";

            var parameters = new DynamicParameters();
            parameters.Add("@Slug", slug);

            var result = await this._conn.QueryAsync<Product>(sql, parameters);
            return result.ToArray().Length > 0;
        }
    }
}
