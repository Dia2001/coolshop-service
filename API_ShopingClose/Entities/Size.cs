using System.ComponentModel.DataAnnotations;

namespace API_ShopingClose.Entities
{
    public class Size
    {
        public Guid SizeID { get; set; }

        [Required]
        public string? SizeName { get; set; }

        public string Description { get; set; }
    }
}
