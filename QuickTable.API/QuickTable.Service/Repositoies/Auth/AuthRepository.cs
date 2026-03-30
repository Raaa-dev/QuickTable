using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuickTable.Service.Models;
using QuickTable.Service.Repositoies.Auth.Dto;

namespace QuickTable.Service.Repositoies.Auth
{
    public class AuthRepository(QuickTableContext _context, IConfiguration _config) : IAuthRepository
    {

        // ─── Login ───────────────────────────────────────────────────────────────
        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == dto.UserName
                                       && u.IsActive == true);

            if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return null;

            return await GenerateTokensAsync(user);
        }


        // ─── Register ────────────────────────────────────────────────────────────
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var exists = await _context.Users
                .AnyAsync(u => u.UserName == dto.UserName);

            if (exists)
                throw new InvalidOperationException("Username already taken.");

            var user = new Models.User
            {
                UserName = dto.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return await GenerateTokensAsync(user);
        }

        // ─── Refresh ─────────────────────────────────────────────────────────────
        public async Task<AuthResponseDto?> RefreshAsync(string refreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken
                                       && u.IsActive == true);

            if (user is null) return null;

            return await GenerateTokensAsync(user);
        }

        // ─── Revoke / Logout ─────────────────────────────────────────────────────
        public async Task RevokeAsync(string refreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user is not null)
            {
                user.RefreshToken = null;
                await _context.SaveChangesAsync();
            }
        }

        // ─── Helpers ─────────────────────────────────────────────────────────────
        private async Task<AuthResponseDto> GenerateTokensAsync(Models.User user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            // ✅ save refresh token directly in Users table
            user.RefreshToken = refreshToken.Token;
            //user.RefreshTokenExpiresAt = refreshToken.ExpiresAt;
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken.Token,
                //AccessTokenExpiresAt = accessToken.ExpiresAt,
                RefreshToken = refreshToken.Token,
                //RefreshTokenExpiresAt = refreshToken.ExpiresAt,
                UserName = user.UserName
            };
        }

        private (string Token, DateTime ExpiresAt) GenerateAccessToken(Models.User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(
                double.Parse(_config["Jwt:AccessTokenExpiresInMinutes"]!));

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }

        private (string Token, DateTime ExpiresAt) GenerateRefreshToken()
        {
            var expires = DateTime.UtcNow.AddDays(
                double.Parse(_config["Jwt:RefreshTokenExpiresDays"]!));

            return (
                Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                expires
            );
        }

        //private AuthResponseDto GenerateToken(Models.User user)
        //{
        //    var key = new SymmetricSecurityKey(
        //        Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var expires = DateTime.UtcNow.AddMinutes(
        //        double.Parse(_config["Jwt:ExpiresInMinutes"]!));

        //    var claims = new[]
        //    {
        //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //    new Claim(ClaimTypes.Name, user.UserName),
        //};

        //    var token = new JwtSecurityToken(
        //        issuer: _config["Jwt:Issuer"],
        //        audience: _config["Jwt:Audience"],
        //        claims: claims,
        //        expires: expires,
        //        signingCredentials: creds
        //    );

        //    return new AuthResponseDto
        //    {
        //        Token = new JwtSecurityTokenHandler().WriteToken(token),
        //        UserName = user.UserName,
        //        ExpiresAt = expires
        //    };
        //}
    }
}