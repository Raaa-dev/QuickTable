using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTable.Service.Repositoies.Auth;
using QuickTable.Service.Repositoies.Auth.Dto;

namespace QuickTable.API.Controller.v1
{
    public class AuthController(IAuthRepository _authRepository, IConfiguration _config, IWebHostEnvironment _env) : BaseController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authRepository.LoginAsync(dto);
            if (result is null)
                return Unauthorized(new { message = "Invalid username or password." });

            SetTokenCookies(result);

            return Ok(new
            {
                message = "Login successful.",
                userName = result.UserName,
                accessToken = result.AccessToken
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var result = await _authRepository.RegisterAsync(dto);
                SetTokenCookies(result);
                return Ok(new
                {
                    message = "Registered successfully.",
                    userName = result.UserName,
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(new { message = "No refresh token found." });

            var result = await _authRepository.RefreshAsync(refreshToken);
            if (result is null)
                return Unauthorized(new { message = "Invalid or expired refresh token." });

            SetTokenCookies(result);

            return Ok(new
            {
                message = "Token refreshed.",
                userName = result.UserName,
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
                await _authRepository.RevokeAsync(refreshToken);

            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return Ok(new { message = "Logged out successfully." });
        }

        private void SetTokenCookies(AuthResponseDto result)
        {
            AppendCookie("accessToken", result.AccessToken,
                DateTimeOffset.UtcNow.AddMinutes(double.Parse(_config["Jwt:AccessTokenExpiresInMinutes"]!)));

            AppendCookie("refreshToken", result.RefreshToken,
                DateTimeOffset.UtcNow.AddDays(double.Parse(_config["Jwt:RefreshTokenExpiresDays"]!)));
        }

        private void AppendCookie(string name, string value, DateTimeOffset expires)
        {
            var isProduction = !_env.IsDevelopment();
            Response.Cookies.Append(name, value, new CookieOptions
            {
                HttpOnly = true,
                Secure = isProduction,
                SameSite = isProduction ? SameSiteMode.None : SameSiteMode.Strict,
                Path = "/",
                Expires = expires
            });
        }
    }
}
