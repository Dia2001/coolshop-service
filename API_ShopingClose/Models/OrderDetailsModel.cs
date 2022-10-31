namespace API_ShopingClose.Models
{
    public class OrderDetailsModel
    {
        public Guid ProductID { get; set; }

        public string SizeID { get; set; }

        public string ColorID { get; set; }

        public int Quantity { get; set; }

        public decimal? Price { get; set; }

        public string? Promotion { get; set; }

        public Guid? OrderID { get; set; }
    }
}
