using Jwt_Token_Validator_Middleware.Models;

namespace Jwt_Token_Validator_Middleware.TokenServices
{
    public interface IToken
    {
        string GenerateJWT(User user);
    }
}
