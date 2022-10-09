using System.ComponentModel.DataAnnotations;

namespace API_ShopingClose.Entities
{
    public class Category
    {
        public long CategoryID { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        public string Description { get; set; }

        public string slug { get; set; }

        public Boolean isShow { get; set; }
    }
}
