namespace Jwt_Token_Validator_Middleware.Models
{
    public class ErrorModel
    {
        public bool Success { get; set; } = false;
        public string Errors { get; set; }
    }
}
