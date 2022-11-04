namespace API_ShopingClose.Models
{
    public class OrderDetailsModel
    {
        public Guid? orderDetailId { get; set; }

        public Guid productId { get; set; }

        public string sizeId { get; set; }

        public string colorId { get; set; }

        public int quantity { get; set; }

        public decimal? price { get; set; }

        public string? promotion { get; set; }

        public Guid? orderId { get; set; }
    }
}
