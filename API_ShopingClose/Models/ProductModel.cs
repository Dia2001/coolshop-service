namespace API_ShopingClose.Model
{
    public class ProductModel
    {
        public Guid productId { get; set; }

        public String name { get; set; }

        public decimal price { get; set; }

        public String description { get; set; }

        public String? slug { get; set; }

        public String? image { get; set; }

        public long brandId { get; set; }

        public long[]? categories { get; set; }

        public Detail? detail { get; set; }

        public int totalQuantity { get; set; }

        public float rate { get; set; }

        public IFormFile? file { get; set; }

    }

    public class Detail
    {
        public string[] sizes { get; set; }

        public string[] colors { get; set; }
    }
}

