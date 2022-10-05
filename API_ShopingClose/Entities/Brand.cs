using System.ComponentModel.DataAnnotations;

namespace API_ShopingClose.Entities
{
    public class Brand
    {
        public Guid BrandID { get; set; }

        [Required]
        public string? BrandName { get; set; }

        public string Description { get; set; }
    }
}
