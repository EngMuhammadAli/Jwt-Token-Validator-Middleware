namespace Jwt_Token_Validator_Middleware.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }


        public int RoleID { get; set; }
        public Role Role { get; set; }
    }
}
