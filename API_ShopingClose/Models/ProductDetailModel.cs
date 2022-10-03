namespace API_ShopingClose.Model;

public class ProductDetailModel
{
    public Guid? productDetailId { get; set; }

    public Guid productId { get; set; }

    public Guid sizeId { get; set; }

    public Guid colorId { get; set; }

    public int quantity { get; set; }
}
