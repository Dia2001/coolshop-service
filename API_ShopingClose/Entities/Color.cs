using System.ComponentModel.DataAnnotations;

namespace API_ShopingClose.Entities
{
    public class Color
    {
        public Guid ColorID { get; set; }

        [Required]
        public string? ColorName { get; set; }

        public string Description { get; set; }
    }
}
