using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.Services
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
            parameters.Add("@OrderID",orderId);
            parameters.Add("@UserID",order.UserID);
            parameters.Add("@OrderstatusID",order.OrderstatusID);
            parameters.Add("@PhoneShip",order.PhoneShip);
            parameters.Add("@AddresShip",order.AddresShip);
            parameters.Add("@NameShip",order.NameShip);
            parameters.Add("@Note",order.Note);
            parameters.Add("@CreateDate",order.CreateDate);
            parameters.Add("@UpdateDate",order.UpdateDate);

            if ( await this._conn.ExecuteAsync(sql, parameters) > 0)
            {
                return orderId;
            }

            return null;
        }

    }
}