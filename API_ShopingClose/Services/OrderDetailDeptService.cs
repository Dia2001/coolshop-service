﻿using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.Service
{
    public class OrderDetailDeptService
    {
        private readonly MySqlConnection _conn;

        public OrderDetailDeptService(MySqlConnection conn)
        {

            _conn = conn;
        }

        public async Task<bool> InsertOrderDetail(OrderDetails orderDetail)
        {
            string sql = "INSERT INTO orderdetail (OrderdetailID, ProductID, ProductName, ProductImage, SizeID, ColorID, Qunatity, Price, Promotion, OrderID)" +
                   "VALUES (@OrderdetailID,@ProductID,@ProductName, @ProductImage, @SizeID,@ColorID,@Qunatity,@Price,@Promotion,@OrderID);";

            var orderDetailId = Guid.NewGuid();
            var parameters = new DynamicParameters();
            parameters.Add("@OrderdetailID", orderDetailId);
            parameters.Add("@ProductID", orderDetail.ProductID);
            parameters.Add("@ProductName", orderDetail.productName);
            parameters.Add("@ProductImage", orderDetail.productImage);
            parameters.Add("@SizeID", orderDetail.SizeID);
            parameters.Add("@ColorID", orderDetail.ColorID);
            parameters.Add("@Qunatity", orderDetail.Qunatity);
            parameters.Add("@Price", orderDetail.Price);
            parameters.Add("@Promotion", orderDetail.Promotion);
            parameters.Add("@OrderID", orderDetail.OrderID);

            return await this._conn.ExecuteAsync(sql, parameters) > 0;

        }

        public async Task<IEnumerable<OrderDetails>> getAllOrderDetailByOrderId(Guid orderId)
        {
            string sql = "SELECT * from orderdetail where OrderID=@OrderID";

            var parameters = new DynamicParameters();
            parameters.Add("@OrderID", orderId);

            return (await _conn.QueryAsync<OrderDetails>(sql, parameters)).ToList();
        }
    }
}
