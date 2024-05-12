using MektepTagamAPI.Data;
using MektepTagamAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MektepTagamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeveloperController : Controller
    {
        private ApplicationDbContext context;
        private RoleManager<IdentityRole> roleManager;
        public DeveloperController(ApplicationDbContext _context, RoleManager<IdentityRole> roleMgr)
        {
            context = _context;
            roleManager = roleMgr;
        }
        [HttpGet]
        [Route("UserRoles")]
        public async Task<IActionResult> UserRoles()
        {
            var list = await context.UserRoles.ToListAsync();
            if (list == null)
            {
                return NotFound(new Response { Status = "Error", Message = "User roles not found" });
            }
            return Ok(list);
        }
        [HttpGet]
        [Route("AspNetRoles")]
        public async Task<IActionResult> AspNetRoles()
        {
            var list = await context.Roles.ToListAsync();
            if (list == null)
            {
                return NotFound(new Response { Status = "Error", Message = "Roles not found" });
            }
            return Ok(list);
        }
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody][Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    // В REST API предпочтительнее возвращать статус 201 Created для успешно созданных ресурсов,
                    // но так как мы не создаем конкретный URI для роли, можно вернуть 200 OK.
                    // Для создания ресурса с URI можно использовать CreatedAtRoute или CreatedAtAction.
                    return Ok(201); // Или CreatedAtAction("GetRole", new { name = name }, null);
                }
                else
                {
                    // Возвращаем список ошибок, если не удалось создать роль
                    return BadRequest(result.Errors);
                }
            }

            // Если модель не проходит валидацию
            return BadRequest(ModelState);
        }
        [HttpDelete]
        [Route("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound("No role found with the given ID.");
            }

            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return NoContent(); // HTTP 204 No Content
            }
            else
            {
                // Возвращаем ошибки, если операция не удалась
                return BadRequest(result.Errors);
            }
        }
    }
}
