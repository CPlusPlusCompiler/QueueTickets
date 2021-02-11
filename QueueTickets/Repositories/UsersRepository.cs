using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QueueTickets.Entities;
using QueueTickets.Helpers;
using QueueTickets.Models;

namespace QueueTickets.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _config;

        public UsersRepository(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        
        public async Task<AuthResponse> Authenticate(AuthRequest request)
        {
            if (request == null)
                return null;

            var user = await GetByUsername(request.Username);

            if (user == null)
                return null;

            // checking if the password is correct by hashing it and comparing to hashed pass from db
            var hashed = EncryptionHelpter.Encrypt(request.Password, user.Password.Salt);

            if (hashed != user.Password.Hash)
                return null;
            else
            {
                var token = GenerateJwtToken(user);
                return new AuthResponse(user, token);
            }
        }


        public async Task<Specialist> GetByUsername(string username)
        {
            var user = await _context.Specialists
                .Where(s => s.Username == username)
                .Include(s => s.Password)
                .FirstAsync();

            return user;
        }


        public async Task<long> Register(RegisterRequest request)
        {
            // checking if username already exists before anything.
            var usernameCount = await _context.Specialists
                .Where(s => s.Username == request.Username)
                .CountAsync();

            if (usernameCount > 0)
                return -1;

            // this will be put to the db
            var saltAndHash = EncryptionHelpter.Encrypt(request.Password);

            var insertedPassword = new SpecialistPassword(saltAndHash.Salt, saltAndHash.Hash);
            _context.Passwords.Add(insertedPassword);
            await _context.SaveChangesAsync();

            var user = new Specialist(
                request.Username, request.Name, request.Surname, insertedPassword.Id);

            _context.Specialists.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        
        public async Task<Specialist> GetById(int userId)
        {
            return await _context.Specialists
                .Where(s => s.Id == userId)
                .Include(s => s.Password)
                .FirstAsync();
        }


        private string GenerateJwtToken(Specialist user)
        {
            // generating token for one day. should be even shorter in real life, but for simplicity this will do for now.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] { new Claim("id", user.Id.ToString()) };
    
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],    
                _config["Jwt:Issuer"],    
                claims,    
                expires: DateTime.Now.AddDays(1),    
                signingCredentials: credentials);    
    
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }
}