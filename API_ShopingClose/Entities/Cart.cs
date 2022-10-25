namespace API_ShopingClose.Entities
{
    public class Cart
    {
        public Guid userId { get; set; }

        public Guid productId { get; set; }

        public string sizeId { get; set; }

        public string colorId { get; set; }

        public string productName { get; set; }

        public string productImage { get; set; }

        public decimal price { get; set; }

        public int quantity { get; set; }

    }
}
