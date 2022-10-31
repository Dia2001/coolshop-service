namespace API_ShopingClose.Entities
{
    public class Order
    {
        public Guid OrderID { get; set; }

        public Guid UserID { get; set; }

        public int OrderstatusID { get; set; }

        public string PhoneShip { get; set; }

        public string AddresShip { get; set; }

        public string NameShip { get; set; }

        public string Note { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
