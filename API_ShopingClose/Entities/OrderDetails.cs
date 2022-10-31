namespace API_ShopingClose.Entities
{
    public class OrderDetails
    {
        public Guid OrderdetailID { get; set; }

        public Guid ProductID { get; set; }

        public string SizeID { get; set; }

        public string ColorID { get; set; }

        public int Qunatity { get; set; }

        public decimal? Price { get; set; }

        public string Promotion { get; set; }

        public Guid OrderID { get; set; }
    }
}
