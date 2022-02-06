using GerenciadorConteudo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorConteudo.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        public readonly DatabaseContext _context;
        private readonly JWTSettings _jwtSettings;

        public AuthController(DatabaseContext context, IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserToken>> Authenticate([FromBody] Usuarios userAuth)
        {
            var user = await _context.Usuarios.FirstAsync
                (
                    u => u.UserEmail == userAuth.UserEmail && u.UserSenha == userAuth.UserSenha
                );
            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            var token = GenerateAccessToken(user);

            return new UserToken
            {
                Usuario = user,
                Token = token
            };
        }

        [HttpPost]
        [Route("loginGoogle")]
        [AllowAnonymous]
        public async Task<ActionResult<UserToken>> AuthenticateGoogle([FromBody] Usuarios userAuth)
        {
            try
            {
                var user = await _context.Usuarios.FirstAsync
                (
                    u => u.UserEmail == userAuth.UserEmail && u.UserSenha == userAuth.UserSenha
                );

                var token = GenerateAccessToken(user);

                return new UserToken
                {
                    Usuario = user,
                    Token = token
                };
            }
            catch (Exception)
            {
                _context.Usuarios.Add(userAuth);
                await _context.SaveChangesAsync();

                var user = await _context.Usuarios.FirstAsync
                (
                    u => u.UserEmail == userAuth.UserEmail && u.UserSenha == userAuth.UserSenha
                );
                var token = GenerateAccessToken(user);

                return new UserToken
                {
                    Usuario = user,
                    Token = token
                };
            }
        }

        private string GenerateAccessToken(Usuarios usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserEmail.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
