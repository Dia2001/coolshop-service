using System.ComponentModel.DataAnnotations;

namespace API_ShopingClose.Entities
{
    public class Galleries
    {
        public long GalleryID { get; set; }

        [Required]
        public Guid ProductID { get; set; }

        public string Thumbnail { get; set; }
    }
}
