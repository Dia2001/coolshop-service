using System.ComponentModel.DataAnnotations;

namespace API_ShopingClose.Entities
{
    public class Role
    {
        /// <summary>
        /// ID Role
        /// </summary>
        [Key]
        public string RoleID { get; set; }

        /// <summary>
        /// Tên role
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string RoleName { get; set; }

    }
}
