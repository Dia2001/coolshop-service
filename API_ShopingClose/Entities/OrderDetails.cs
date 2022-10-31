namespace API_ShopingClose.Entities
{
    public class OrderDetails
    {
        public Guid OrderdetailID { get; set; }

        public Guid  ProductID { get; set; }

        public int SizeID { get; set; }

        public int ColorID { get; set; }

        public int Qunatity { get; set; }

        public decimal Price { get; set; }

        public string Promotion { get; set; }

        public Guid OrderID { get; set; }
    }
}
