namespace API_ShopingClose.Model;

public class ProductDetailModel
{
    public Guid? productDetailId { get; set; }

    public Guid productId { get; set; }

    public string sizeId { get; set; }

    public string colorId { get; set; }

    public int quantity { get; set; }
}
