using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_ShopingClose.Entities
{
    public class User
    {
        #region Property

        /// <summary>
        /// ID user
        /// </summary>
        [Key]
        public Guid UserID { get; set; }

        /// <summary>
        /// Tên đăng nhập user
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string Username { get; set; }

        /// <summary>
        /// Mật khẩu user
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string PassWord { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Họ tên đầy đủ
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string FullName { get; set; }

        /// <summary>
        /// Ngày đăng nhập gần nhất
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public DateTime LastOperatingTime { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Ngày sửa gần nhất
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Ngày xóa gần nhất
        /// </summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// ID Role
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string RoleID { get; set; }

        #endregion
    }
}
