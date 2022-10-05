using System.ComponentModel.DataAnnotations;

namespace API_ShopingClose.Entities
{
    public class Category
    {
        public Guid CategoryID { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        public string Description { get; set; }
    }
}
