using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.Service
{
    public class OrderDeptService
    {
        private readonly MySqlConnection _conn;

        public OrderDeptService(MySqlConnection conn)
        {

            _conn = conn;
        }

        public async Task<Guid?> InsertOrder(Order order)
        {
            string sql = "INSERT INTO orders (OrderID, UserID, OrderstatusID, PhoneShip, AddresShip, NameShip, Note, CreateDate, UpdateDate)" +
                   "VALUES (@OrderID,@UserID,@OrderstatusID,@PhoneShip,@AddresShip,@NameShip,@Note,@CreateDate,@UpdateDate);";

            var orderId = Guid.NewGuid();
            var parameters = new DynamicParameters();
            parameters.Add("@OrderID", orderId);
            parameters.Add("@UserID", order.UserID);
            parameters.Add("@OrderstatusID", order.OrderstatusID);
            parameters.Add("@PhoneShip", order.PhoneShip);
            parameters.Add("@AddresShip", order.AddresShip);
            parameters.Add("@NameShip", order.NameShip);
            parameters.Add("@Note", order.Note);
            parameters.Add("@CreateDate", order.CreateDate);
            parameters.Add("@UpdateDate", order.UpdateDate);

            if (await this._conn.ExecuteAsync(sql, parameters) > 0)
            {
                return orderId;
            }

            return null;
        }

        public async Task<Order> getOneOrderByUserIdAndOrderId(Guid userId, Guid orderId)
        {
            string sql = "SELECT * FROM orders where UserID=@UserID and OrderID = @OrderID";

            var parameters = new DynamicParameters();

            parameters.Add("@UserID", userId);
            parameters.Add("@OrderID", orderId);

            var result = await _conn.QueryAsync<Order>(sql, parameters);
            return result.FirstOrDefault();
        }

        public async Task<Order> getOrderById(Guid orderId)
        {
            string sql = "SELECT * FROM orders where OrderID = @OrderID";
            var parameters = new DynamicParameters();
            parameters.Add("@OrderID", orderId);

            var result = await _conn.QueryAsync<Order>(sql, parameters);
            return result.FirstOrDefault();
        }

        public async Task<bool> updateOrder(Order order)
        {

            string sql = "UPDATE orders set " +
                   " OrderstatusID = @OrderstatusID, PhoneShip = @PhoneShip, AddresShip = @AddresShip, NameShip = @NameShip, Note = @Note, CreateDate = @CreateDate, UpdateDate = @UpdateDate " +
                   " where OrderID = @OrderID";

            var parameters = new DynamicParameters();
            parameters.Add("@OrderID", order.OrderID);
            parameters.Add("@OrderstatusID", order.OrderstatusID);
            parameters.Add("@PhoneShip", order.PhoneShip);
            parameters.Add("@AddresShip", order.AddresShip);
            parameters.Add("@NameShip", order.NameShip);
            parameters.Add("@Note", order.Note);
            parameters.Add("@CreateDate", order.CreateDate);
            parameters.Add("@UpdateDate", order.UpdateDate);

            return await this._conn.ExecuteAsync(sql, parameters) > 0;
        }

        public async Task<IEnumerable<Order>> getAllOrder()
        {
            string sql = "SELECT * FROM orders";
            return (await _conn.QueryAsync<Order>(sql)).ToList();
        }

        public async Task<IEnumerable<Order>> getAllOrderToUser(Guid userId)
        {
            string sql = "SELECT * FROM orders where UserID=@UserID";
            var parameters = new DynamicParameters();
            parameters.Add("UserID", userId);
            return (await _conn.QueryAsync<Order>(sql, parameters)).ToList();
        }

        public async Task<bool> DeleteOrder(Guid orderId)
        {
            string sql = "DELETE FROM order WHERE OrderID=@OrderID";

            var parameters = new DynamicParameters();
            parameters.Add("@OrderID", orderId);

            return await this._conn.ExecuteAsync(sql, parameters) > 0;
        }
    }
}
