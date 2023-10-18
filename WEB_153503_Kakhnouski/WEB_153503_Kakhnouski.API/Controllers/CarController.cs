﻿using Microsoft.AspNetCore.Mvc;
using WEB_153503_Kakhnouski.Domain.Entities;
using WEB_153503_Kakhnouski.Domain.Models;
using WEB_153503_Kakhnouski.Services.CarService;

namespace WEB_153503_Kakhnouski.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        // GET: api/Cars
        [HttpGet("{pageNo:int}")]
        [HttpGet("{category?}/{pageNo:int?}/")]
        public async Task<ActionResult<ResponseData<List<Car>>>> GetCars(string? category, int pageNo = 1, int pageSize = 3)
        {
            var result = await _carService.GetCarListAsync(category, pageNo, pageSize);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET: api/Cars/car5
        [HttpGet("car{id}")]
        public async Task<ActionResult<ResponseData<Car>>> GetCar(int id)
        {
            var result = await _carService.GetCarByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseData<Car>>> PutCar(int id, Car car)
        {
            try
            {
                await _carService.UpdateCarAsync(id, car);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseData<Car>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }

            return Ok(new ResponseData<Car>()
            {
                Data = car
            });
        }

        // POST: api/Cars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResponseData<Car>>> PostCar(Car car)
        {
            var result = await _carService.CreateCarAsync(car);
            return result.Success ? Ok(result.Data) : BadRequest(result);
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                await _carService.DeleteCarAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseData<Car>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }

            return NoContent();
        }

        private async Task<bool> CarExists(int id)
        {
            return (await _carService.GetCarByIdAsync(id)).Success;
        }
    }
}
