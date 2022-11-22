﻿using API_ShopingClose.Common;
using API_ShopingClose.Entities;
using API_ShopingClose.Models;
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

        public async Task<bool> UpdateProduct(Product product, string productID)
        {
            bool b = false;
            Guid productid = Guid.Parse(productID);
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
            parameters.Add("@ProductID", productid);
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

        public async Task<Dictionary<String, dynamic>?> getFilterProduct(
             string? keyword,
             long? brandId,
             int pageSize,
             int pageNumber)
        {

            string storedProcedureName = "Proc_product_GetPaging";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@v_Offset", (pageNumber - 1) * pageSize);
            parameters.Add("@v_Limit", pageSize);
            parameters.Add("@v_Sort", "Rate DESC");

            var orConditions = new List<string>();
            var andConditions = new List<string>();
            string whereClause = "";

            if (keyword != null)
            {
                orConditions.Add($"ProductName LIKE '%{keyword}%'");
            }
            if (orConditions.Count > 0)
            {
                whereClause = $"({string.Join(" OR ", orConditions)})";
            }
            if (brandId != null)
            {
                andConditions.Add($"BrandID LIKE '%{brandId}%'");
            }

            if (andConditions.Count > 0)
            {
                if (whereClause != "")
                {
                    whereClause += $" AND {string.Join(" AND ", andConditions)}";
                }
                else
                {
                    whereClause += $"{string.Join(" AND ", andConditions)}";
                }
            }

            parameters.Add("@v_Where", whereClause);

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var multipleResults = _conn.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            if (multipleResults != null)
            {
                var products = (await multipleResults.ReadAsync<Product>()).ToList();
                var totalCount = (await multipleResults.ReadAsync<long>()).Single();

                Dictionary<String, dynamic> hash = new Dictionary<string, object>();
                hash.Add("products", products);
                hash.Add("totalCount", totalCount);

                return hash;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>> getAllProducts()
        {
            string sql = "select * from product";
            return await _conn.QueryAsync<Product>(sql);
        }

        public async Task<Product?> getOneProduct(string productIdorSlug)
        {
            Guid productId = new Guid();
            var parameters = new DynamicParameters();
            string sql = "select * from product where ProductID=@ProductID";
            if (Guid.TryParse(productIdorSlug, out productId))
            {
                parameters.Add("@ProductID", productId);
            }
            else
            {
                sql = "select * from product where Slug=@Slug";
                parameters.Add("@Slug", productIdorSlug);
            }

            var result = await this._conn.QueryAsync<Product>(sql, parameters);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Product>> getProductSmilarByIdProduct(Guid productId)
        {
            Product product = await getOneProduct(productId.ToString());

            string whereClause = "";
            string productname = product.ProductName.NonUnicode().ToLower();
            string[] wordsproduct = productname.Split(' ');

            foreach (var oneWordproduct in wordsproduct)
            {
                if (whereClause != "")
                {
                    whereClause += $" or {string.Join(" or ", $"product.ProductName  LIKE '%{oneWordproduct}%'")}";
                }
                else
                {
                    whereClause += $"{string.Join(" or ", $"product.ProductName  LIKE '%{oneWordproduct}%'")}";
                }
            }

            string sql = "select * from product " +
               "inner join productincategory on product.ProductID=productincategory.ProductID " +
               "where " + whereClause +
               " AND  @SmallestValue<=product.Price AND product.Price<=@GreatestValue " +
               "AND productincategory.CategoryID IN(SELECT productincategory.CategoryID FROM productincategory WHERE  productincategory.ProductID=@ProductID) " +
               "GROUP BY product.ProductID, productincategory.CategoryID " +
               "limit 10";


            Console.WriteLine(sql);


            decimal priceSmallest = product.Price - decimal.Parse("100000.00");
            decimal priceGreatest = product.Price + decimal.Parse("100000.00");

            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", product.ProductName);
            parameters.Add("@SmallestValue", priceSmallest);
            parameters.Add("@GreatestValue", priceGreatest);
            parameters.Add("@ProductID", product.ProductID);

            return (await this._conn.QueryAsync<Product>(sql, parameters)).ToList();
        }

        public async Task<IEnumerable<Category>> getCategoryToProductBestSelling()
        {
            string sql = "SELECT category.CategoryID, category.CategoryName FROM category " +
                        "LEFT JOIN productincategory on category.CategoryID = productincategory.CategoryID " +
                        "WHERE productincategory.ProductID IN(SELECT SUBQUERY.ProductID FROM (SELECT orderdetail.ProductID, SUM(orderdetail.Qunatity) AS Qunatity FROM orders " +
                        "INNER JOIN orderdetail on orders.OrderID= orderdetail.OrderID " +
                        "WHERE orders.OrderstatusID= 'deliveredToTransporter' " +
                        "GROUP BY ProductID " +
                        "ORDER BY Qunatity DESC " +
                        "LIMIT 3) AS SUBQUERY) " +
                        "GROUP BY category.CategoryID LIMIT 3; ";
            return (await this._conn.QueryAsync<Category>(sql)).ToList();
        }

        public async Task<IEnumerable<Product>> getProductBestSellingToCategory(long categoryId)
        {
            string sql = "SELECT * FROM product " +
                        "LEFT JOIN productincategory on product.ProductID = productincategory.ProductID " +
                        "WHERE productincategory.CategoryID =" + categoryId +
                        " GROUP BY product.ProductID LIMIT 6;";

            return (await this._conn.QueryAsync<Product>(sql)).ToList();
        }

        public async Task<IEnumerable<TurnoverModel>> getTurnover(DateTime startDate,DateTime endDate)
        {
            string sql = "SELECT  CAST(orders.CreateDate AS DATE) AS dates,COUNT(orders.CreateDate) AS OrderNumber, SUM(orderdetail.Price) AS Turnover FROM orders " +
                         "INNER JOIN orderdetail on orders.OrderID = orderdetail.OrderID " +
                         "WHERE orders.CreateDate IN(SELECT orders.CreateDate FROM orders " +
                                                    "WHERE CreateDate >='" +startDate.ToString("yyyy-MM-dd HH:mm:ss") +
                                                    "' AND CreateDate <='" + endDate.ToString("yyyy-MM-dd HH:mm:ss") +
                                                    "' GROUP BY orders.CreateDate)" +
                         "GROUP BY dates;";
            return (await this._conn.QueryAsync<TurnoverModel>(sql)).ToList();
        }
    }
}
