using API_ShopingClose.Entities;

namespace API_ShopingClose.Model;

public class GalleryModel
{
    public long galleryId { get; set; }

    public Guid productId { get; set; }

    public string thumbnail { get; set; }

    public GalleryModel(Galleries gallery)
    {
        galleryId = gallery.GalleryID;
        productId = gallery.ProductID;
        thumbnail = gallery.Thumbnail;
    }
}
