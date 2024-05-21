using MektepTagamAPI.Authentication;
using MektepTagamAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MektepTagamAPI.Authenticate.Models;
using MektepTagamAPI.Models;
using MektepTagamAPI.Authenticate;
using Microsoft.AspNetCore.Authorization;

namespace APIAvtoMig.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private ApplicationDbContext context;
        public AuthenticateController(
            UserManager<AspNetUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, ApplicationDbContext _context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            context = _context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.PhoneNumber),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

                // Добавление ролей в claims
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = await GetToken(authClaims, user.Id, user.OrganizationId); // Ожидание выполнения асинхронного метода GetToken

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
        [HttpPost]
        [Route("StudentLogin")]
        public async Task<IActionResult> StudentLogin([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.PhoneNumber),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

                // Добавление ролей в claims
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = await GetToken(authClaims, user.Id, user.OrganizationId); // Ожидание выполнения асинхронного метода GetToken

                if (!userRoles.Contains("Пользователь"))
                {
                    return BadRequest("Текущий пользователь не ученик");
                }
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            AspNetUser user = new()
            {
                Email = model.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.PhoneNumber,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                SurName = model.SurName,
                IndividualNumber = model.IndividualNumberUser
            };

            var checkOrganizationExists = await context.Organizations.FirstOrDefaultAsync(x => x.Number == model.OrganizationId);
            if (checkOrganizationExists != null)
            {
                user.OrganizationId = checkOrganizationExists.Id;

                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "Пользователь");

                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

                await context.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
            return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Organization does not exists!" });
        }

        private async Task<JwtSecurityToken> GetToken(List<Claim> authClaims, string userId, int? organizationId)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            // Получение пользователя и его ролей
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);

            // Использование HashSet для исключения дублирования ролей
            var roleClaims = new HashSet<Claim>(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Добавление ролей в claims
            authClaims.AddRange(roleClaims);

            // Добавление других claims, если это необходимо
            if (organizationId.HasValue)
            {
                var organization = await context.Organizations.FindAsync(organizationId.Value);
                authClaims.Add(new Claim("OrganizationId", organizationId.Value.ToString()));
                authClaims.Add(new Claim("Id", userId));
            }

            // Создание JWT токена
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(2190),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }



    }
}
