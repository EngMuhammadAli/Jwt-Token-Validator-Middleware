using Jwt_Token_Validator_Middleware.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt_Token_Validator_Middleware.Middleware
{
    public class TokenValidatorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenValidationParameters _tokenValidationParams;

        public TokenValidatorMiddleware(RequestDelegate next, TokenValidationParameters tokenValidationParams)
        {
            _next = next;
            _tokenValidationParams = tokenValidationParams;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var token = context.Request.Cookies["Authorization"];

                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var tokenInVerification = jwtTokenHandler.ValidateToken(token, _tokenValidationParams, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var error = new ErrorModel()
                        {
                            Success = false,
                            Errors = "Token is Invalid"
                        };
                        context.Items["Error"] = error;
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Token is Invalid");
                        return;
                    }
                }
            }
            catch (SecurityTokenExpiredException)
            {
                var error = new ErrorModel()
                {
                    Success = false,
                    Errors = "Token has expired."
                };
                context.Items["Error"] = error;
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token has expired");

                // Redirect to /Login/Login on token expiration
                if (!context.Response.HasStarted)
                {
                    context.Response.Redirect("/Login/Login");
                }
                return;
            }
            catch (Exception)
            {
                var error = new ErrorModel()
                {
                    Success = false,
                    Errors = "Token validation failed."
                };
                context.Items["Error"] = error;
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token validation failed");

                // Redirect to /Login/Login on token validation failure
                if (!context.Response.HasStarted)
                {
                    context.Response.Redirect("/Login/Login");
                }
                return;
            }

            await _next(context);
        }
    }
}
