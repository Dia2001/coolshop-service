namespace API_ShopingClose.Models
{
    public class ProductdetailQuantity
    {
        public Guid productId { get; set; }

        public string sizeId { get; set; }

        public string colorId { get; set; }

        public int quantity { get; set; }
    }
}
