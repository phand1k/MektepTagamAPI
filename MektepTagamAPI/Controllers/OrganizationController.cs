using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MektepTagamAPI.Authenticate;
using MektepTagamAPI.Models;
using MektepTagamAPI.Authentication;
using MektepTagamAPI.Data;

namespace MektepTagamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : Controller
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly ApplicationDbContext _context;
        public OrganizationController(UserManager<AspNetUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Organization organization)
        {
            var organizationExists = await _context.Organizations
                .Where(x => x.Number == organization.Number)
                .FirstOrDefaultAsync();

            if (organizationExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Organization already exists!" });

            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
            AspNetUser defaultUser = new()
            {
                Email = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = Guid.NewGuid().ToString(),
                PhoneNumber = Guid.NewGuid().ToString(),
                FirstName = "Default",
                LastName = "User",
                PasswordHash = Guid.NewGuid().ToString(),
                NormalizedEmail = Guid.NewGuid().ToString(),
            };

            defaultUser.OrganizationId = organization.Id;

            await _context.AspNetUsers.AddAsync(defaultUser);

            await _context.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "Organization created successfully!" });
        }

    }
}
