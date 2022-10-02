using System.ComponentModel.DataAnnotations;

namespace API_ShopingClose.Entities;

public class ProductDetails
{

    public Guid productDetailsId { get; set; }

    [Required]
    public Guid? productId { get; set; }

    [Required]
    public Guid sizeId { get; set; }

    [Required]
    public Guid colorId { get; set; }

    public int quantity { get; set; }
}
