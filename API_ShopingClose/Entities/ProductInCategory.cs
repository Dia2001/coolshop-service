using System.ComponentModel.DataAnnotations;

namespace API_ShopingClose.Entities;

public class ProductInCategory
{
    [Required]
    public Guid? productId { get; set; }

    [Required]
    public long categoryId { get; set; }
}
