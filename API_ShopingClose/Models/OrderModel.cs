namespace API_ShopingClose.Models
{
    public class OrderModel
    {
        public Guid? orderId { get; set; }

        public Guid? userId { get; set; }

        public string? orderStatusId { get; set; }

        public string phoneShip { get; set; }

        public string addressShip { get; set; }

        public string nameShip { get; set; }

        public string note { get; set; }

        public DateTime? createDate { get; set; }

        public DateTime? updateDate { get; set; }

        public OrderDetailsModel[] orderDetail { get; set; }
    }
}
