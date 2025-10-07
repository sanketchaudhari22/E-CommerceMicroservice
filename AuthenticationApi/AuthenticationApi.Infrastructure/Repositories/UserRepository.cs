using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interface;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using E_CommerceSharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Infrastructure.Repositories
{
    internal class UserRepository : IUserInterface
    {
        private readonly AuthenticationDbContext _context;
        private readonly IConfiguration _config;

        public UserRepository(AuthenticationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AppUser?> GetUserByEmail(string email)
        {
            return await _context.users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<GetUserDto> GetUser(int userId)
        {
            var user = await _context.users.FindAsync(userId);
            if (user is null)
                throw new Exception("User not found");

            return new GetUserDto(
                user.Id,
                user.Name!,
                user.TelephoneNumber!,
                user.Password!,
                user.Address!,
                user.Email!,
                user.Role!
            );
        }

        public async Task<Response> Login(LoginDto loginDto)
        {
            var getUser = await GetUserByEmail(loginDto.Email);
            if (getUser is null)
                return new Response(false, "Invalid credentials");

            bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, getUser.Password);
            if (!verifyPassword)
                return new Response(false, "Invalid credentials");

            string token = GenerateToken(getUser);
            return new Response(true, "Login successful", token);
        }

        private string GenerateToken(AppUser user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Authentication:Key"]!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name!),
                new(ClaimTypes.Email, user.Email!)
            };

            if (!string.IsNullOrEmpty(user.Role))
                claims.Add(new(ClaimTypes.Role, user.Role!));

            var token = new JwtSecurityToken(
                issuer: _config["Authentication:Issuer"],
                audience: _config["Authentication:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Response> Register(AppUserDto appUserDto)
        {
            var getUser = await GetUserByEmail(appUserDto.Email);
            if (getUser is not null)
                return new Response(false, "Email already registered");

            var newUser = new AppUser
            {
                Name = appUserDto.Name,
                Email = appUserDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(appUserDto.Password),
                TelephoneNumber = appUserDto.TelephoneNumber,
                Address = appUserDto.Address,
                Role = appUserDto.Role
            };

            _context.users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser.Id > 0
                ? new Response(true, "User registered successfully")
                : new Response(false, "Invalid data provided");
        }
    }
}
