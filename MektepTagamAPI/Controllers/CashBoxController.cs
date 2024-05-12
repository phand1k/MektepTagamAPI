using MektepTagamAPI.Authentication;
using MektepTagamAPI.Data;
using MektepTagamAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Generic;
using System.Security.Claims;

namespace MektepTagamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Кассир")]
    public class CashBoxController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AspNetUser> _userManager;
        public CashBoxController(ApplicationDbContext context, UserManager<AspNetUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        [Route("GetDataAspNetUsers")]
        public async Task<IActionResult> GetDataAspNetUsers()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var userId = await _context.AspNetUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Важно дождаться выполнения FirstOrDefaultAsync перед тем, как использовать результат
            var aspNetUsersData = await _context.AspNetUsers
                .Where(x => x.OrganizationId == user.OrganizationId)
                .ToListAsync();

            if (aspNetUsersData == null)
            {
                return BadRequest();
            }

            // Убедитесь, что organizationData может быть сериализован
            return Ok(aspNetUsersData);
        }
        [Authorize]
        [HttpGet]
        [Route("GetOrganization")]
        public async Task<IActionResult> GetDataOrganization(int? organizationId)
        {
            if (organizationId == null)
            {
                return NotFound(new Response { Status = "Error", Message = "Id not found" });
            }
            var organization = await _context.Organizations
                                            .Where(x => x.Id == organizationId)
                                            .ToListAsync(); // Изменили FirstOrDefaultAsync на ToListAsync
            if (organization == null || organization.Count == 0) // Проверяем, что список не пуст
            {
                return NotFound(new Response { Status = "Error", Message = "Organization not found" });
            }
            return Ok(organization); // Возвращаем список организаций
        }
        [HttpGet]
        [Route("GetAllDishes")]
        public async Task<IActionResult> GetAllDishes()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var userId = await _context.AspNetUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var allDishesData = await _context.Dishes.Where(x => x.OrganizationId == user.OrganizationId).ToListAsync();
            if (allDishesData == null)
            {
                return BadRequest();
            }
            return Ok(allDishesData);
        }

        [HttpGet]
        [Route("GetBalance")]
        public async Task<IActionResult> GetBalance(string? code)
        {
            if (code == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Car code is null!" });
            }

            double? summ = _context.Transactions.Include(x=>x.CardCode)
                         .Where(t => t.CardCode.Code == code && t.IsDeleted == false)
                         .Sum(t => (double?)t.Amount) ?? 0.0;
            if (summ == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Balance not found!" });
            }
            return Ok(summ);
        }
        [HttpGet]
        [Route("GetAllTransactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var userId = await _context.AspNetUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var allTransactionsData = await _context.Transactions.Where(x=>x.OrganizationId == user.OrganizationId).ToListAsync();
            if (allTransactionsData == null)
            {
                return BadRequest();
            }
            return Ok(allTransactionsData);
        }
        [HttpGet]
        [Route("GetTransactions")]
        public async Task<IActionResult> GetTransactions(string? code)
        {
            if (code == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Car code is null!" });
            }
            var list = await _context.Transactions.Include(x=>x.CardCode).Where(x=>x.CardCode.Code == code).ToListAsync();
            if (list == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Transactions not found!" });
            }
            return Ok(list);
        }
        [HttpPost]
        [Route("CreateTransaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var userId = await _context.AspNetUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            transaction.OrganizationId = user.OrganizationId;
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "Transaction created successfully!" });
        }
        [HttpGet]
        [Route("GetAllCardCodes")]
        public async Task<IActionResult> GetAllCardCodes()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var userId = await _context.AspNetUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var allCardCodes = await _context.CardCodes.Where(x=>x.OrganizationId == user.OrganizationId).ToListAsync();
            if (allCardCodes == null)
            {
                return BadRequest();
            }
            return Ok(allCardCodes);
        }
    }
}
