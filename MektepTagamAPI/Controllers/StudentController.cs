using MektepTagamAPI.Authentication;
using MektepTagamAPI.Data;
using MektepTagamAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MektepTagamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Пользователь")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AspNetUser> _userManager;
        public StudentController(ApplicationDbContext context, UserManager<AspNetUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize]
        [HttpGet]
        [Route("GetQR")]
        public async Task<IActionResult> GetQR()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var userId = await _context.AspNetUsers.
                Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefaultAsync();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.Id);
        }
        [HttpGet]
        [Route("GetProfileInfo")]
        public async Task<IActionResult> GetProfileInfo()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var userId = await _context.AspNetUsers.
                Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefaultAsync();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
            { 
                return NotFound();
            }
            return Ok(user.FullName);
        }
        [HttpGet]
        [Route("GetTransactions")]
        public async Task<IActionResult> GetTransactions(string? code)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var userId = await _context.AspNetUsers.
                Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefaultAsync();

            var user = await _userManager.FindByIdAsync(userId);

            var transactions = _context.Transactions.Include(x=>x.Dish).Include(x => x.CardCode.AspNetUser)
                         .Where(t => t.CardCode.Code == code || t.CardCode.AspNetUserId == user.Id).
                         Where(x => x.IsDeleted == false).OrderByDescending(x=>x.DateOfCreatedTransaction).Take(5);

            if (transactions == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Transactions not found!" });
            }
            return Ok(transactions);
        }
        [HttpGet]
        [Route("GetBalance")]
        public async Task<IActionResult> GetBalance(string? code)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var userId = await _context.AspNetUsers.
                Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefaultAsync();

            var user = await _userManager.FindByIdAsync(userId);

            double? summ = _context.Transactions.Include(x => x.CardCode.AspNetUser)
                         .Where(t => t.CardCode.Code == code || t.CardCode.AspNetUserId == user.Id).Where(x=>x.IsDeleted == false)
                         .Sum(t => (double?)t.Amount) ?? 0.0;

            if (summ == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Balance not found!" });
            }
            return Ok(summ);
        }

    }
}
