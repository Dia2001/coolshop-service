namespace API_ShopingClose.Models
{
    public class OrderModel
    {
        public Guid OrderID { get; set; }

        /*public string UserID { get; set; }*/

        public int OrderstatusID { get; set; }

        public string PhoneShip { get; set; }

        public string AddresShip { get; set; }

        public string NameShip { get; set; }

        public string Note { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public OrderDetail[] OrderDetail { get; set; }
    }
    public class OrderDetail
    {
        public Guid ProductID { get; set; }

        public int SizeID { get; set; }

        public int ColorID { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string Promotion { get; set; }

        public Guid OrderID { get; set; }
    }
}
