using System.ComponentModel.DataAnnotations;

namespace API_ShopingClose.Entities
{
    public class User
    {
        #region Property

        /// <summary>
        /// ID user
        /// </summary>
        [Key]
        public Guid User_ID { get; set; }

        /// <summary>
        /// Tên đăng nhập user
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string User_Name { get; set; }

        /// <summary>
        /// Mật khẩu user
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string PassWord { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Phone_Number { get; set; }

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
        public DateTime Last_Operating_Time { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime Created_Date { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string Created_By { get; set; }

        /// <summary>
        /// Ngày sửa gần nhất
        /// </summary>
        public DateTime Modified_Date { get; set; }

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        public string Modified_By { get; set; }

        /// <summary>
        /// Ngày xóa gần nhất
        /// </summary>
        public string Delete_date { get; set; }

        /// <summary>
        /// ID Role
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public Guid Role_ID { get; set; }

        #endregion
    }
}
