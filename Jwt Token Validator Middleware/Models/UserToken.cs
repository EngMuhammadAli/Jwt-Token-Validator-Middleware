using System.ComponentModel.DataAnnotations;

namespace Jwt_Token_Validator_Middleware.Models
{
    public class UserToken
    {
        [Key]
        public int UserTokenID { get; set; }
        public int UserID { get; set; }
        public string Token { get; set;}
    }
}
