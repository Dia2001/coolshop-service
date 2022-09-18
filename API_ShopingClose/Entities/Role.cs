using System.ComponentModel.DataAnnotations;

namespace API_ShopingClose.Entities
{
    public class Role
    {
        /// <summary>
        /// ID Role
        /// </summary>
        [Key]
        public Guid Role_ID { get; set; }

        /// <summary>
        /// Tên role
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string Role_Name { get; set; }
    }
}
