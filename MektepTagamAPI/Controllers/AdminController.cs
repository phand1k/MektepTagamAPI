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

    }
}
