using System.ComponentModel.DataAnnotations;

namespace Jwt_Token_Validator_Middleware.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
