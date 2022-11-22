using API_ShopingClose.Entities;

namespace API_ShopingClose.Models
{
    public class FeaturedProductListModel
    {
        public long categoryId { get; set; }

        public string? name { get; set; }

        public List<Product>? product { get; set; } 

    }
}
