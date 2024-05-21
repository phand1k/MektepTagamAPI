using MektepTagamAPI.Models;
using MektepTagamAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MektepTagamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        }
        [Route("GetCar")]
        [HttpGet]
        public async Task<IActionResult> GetCar(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }
        [Route("GetAllCars")]
        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            if (cars == null)
            {
                return NotFound();
            }
            return Ok(cars);
        }
        [Route("CreateCar")]
        [HttpPost]
        public async Task<IActionResult> CreateCar(Car car)
        {
            if (ModelState.IsValid && await _carService.CreateCarAsync(car) != false)
            {
                await _carService.CreateCarAsync(car);
                return Ok(car);
            }
            return BadRequest();
        }
    }
}
