using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly AppDbContext _context;

    public CarsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/cars
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Car>>> GetCars()
    {
        var cars = await _context.Cars
            .Include(c => c.Parts)
            .ToListAsync();
        return Ok(cars);
    }

    // GET: api/cars/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Car>> GetCar(int id)
    {
        var car = await _context.Cars
            .Include(c => c.Parts)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (car == null)
        {
            return NotFound();
        }

        return Ok(car);
    }

    // POST: api/cars
    [HttpPost]
    public async Task<ActionResult<Car>> PostCar(Car car)
    {
        // Валидация
        if (string.IsNullOrWhiteSpace(car.Name))
            return BadRequest("Car name is required");
        if (string.IsNullOrWhiteSpace(car.Brand))
            return BadRequest("Brand is required");
        if (car.Year < 1900 || car.Year > DateTime.Now.Year + 1)
            return BadRequest("Invalid year");
        if (car.Price < 0)
            return BadRequest("Price must be positive");

        try
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCar", new { id = car.Id }, car);
        }
        catch (Exception ex)
        {
            return BadRequest("Error saving car: " + ex.Message);
        }
    }

    // PUT: api/cars/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCar(int id, Car car)
    {
        if (id != car.Id)
        {
            return BadRequest();
        }

        _context.Entry(car).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CarExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/cars/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CarExists(int id)
    {
        return _context.Cars.Any(e => e.Id == id);
    }
}
