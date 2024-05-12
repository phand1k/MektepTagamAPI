using MektepTagamAPI.Authenticate;
using MektepTagamAPI.Authenticate.Models;
using MektepTagamAPI.Authentication;
using MektepTagamAPI.Data;
using MektepTagamAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MektepTagamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private ApplicationDbContext context;
        private RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AspNetUser> userManager;

        public AdminController(ApplicationDbContext _context, RoleManager<IdentityRole> roleManager, UserManager<AspNetUser> userManager)
        {
            context = _context;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpPost]
        [Route("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    ModelState.AddModelError("UserId", "User not found.");
                    return BadRequest(ModelState);
                }

                // Находим роль по RoleId
                var newRole = await roleManager.FindByIdAsync(model.RoleId);
                if (newRole == null)
                {
                    ModelState.AddModelError("RoleId", "Role not found.");
                    return BadRequest(ModelState);
                }

                // Удаление всех текущих ролей пользователя
                var currentRoles = await userManager.GetRolesAsync(user);
                var removalResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removalResult.Succeeded)
                {
                    return BadRequest(removalResult.Errors);
                }

                // Добавление пользователя в новую роль
                var addRoleResult = await userManager.AddToRoleAsync(user, newRole.Name);
                if (!addRoleResult.Succeeded)
                {
                    return BadRequest(addRoleResult.Errors);
                }

                return Ok(new { message = $"User role updated to {newRole.Name}" });
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("CreateDish")]
        public async Task<IActionResult> CreateDish([FromBody] Dish dish)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var userId = await context.AspNetUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var dishExists = await context.Dishes.
                Where(x => x.Name == dish.Name && x.IsDeleted == false).Where(x=>x.OrganizationId == user.OrganizationId).FirstOrDefaultAsync();
            if (dishExists == null)
            {
                dish.OrganizationId = user.OrganizationId;
                await context.Dishes.AddAsync(dish);

                await context.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Dish created successfully!" });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Dish exists!" });
        }
        [HttpPost]
        [Route("CreateCardCode")]
        public async Task<IActionResult> CreateCardCode([FromBody] CardCode cardCode)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var userId = await context.AspNetUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var cardCodeExists = await context.CardCodes.
                Where(x => x.Code == cardCode.Code && x.IsDeleted == false).FirstOrDefaultAsync();
            if (cardCodeExists == null)
            {
                cardCode.OrganizationId = user.OrganizationId;
                await context.CardCodes.AddAsync(cardCode);

                await context.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Card code created successfully!" });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Card code exists!" });
        }
    }
}
