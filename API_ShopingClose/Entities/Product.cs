using API_ShopingClose.Enums;
using System.ComponentModel.DataAnnotations;

namespace API_ShopingClose.Entities
{
    public class Product
    {
        /// <summary>
        /// Id sản phẩm
        /// </summary>
        [Key]
        public Guid ProductID { get; set; }

        /// <summary>
        /// ID nhãn hiệu
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public Guid BrandID { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string ProductName { get; set; }

        /// <summary>
        /// Giá sản phẩm
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string Price { get; set; }

        /// <summary>
        /// Hình ảnh sản phẩm
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string Image { get; set; }

        /// <summary>
        /// Đánh giá sản phẩm
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public Rate Rate { get; set; }

        /// <summary>
        /// Link SEO
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Mô tả sản phẩm
        /// </summary>
        public string Description { get; set; }
    }
}
