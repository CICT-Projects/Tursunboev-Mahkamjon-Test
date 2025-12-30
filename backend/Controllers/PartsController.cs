using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PartsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/parts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Part>>> GetParts()
    {
        var parts = await _context.Parts.ToListAsync();
        return Ok(parts);
    }

    // GET: api/parts/car/5
    [HttpGet("car/{carId}")]
    public async Task<ActionResult<IEnumerable<Part>>> GetPartsByCar(int carId)
    {
        var parts = await _context.Parts
            .Where(p => p.CarId == carId)
            .ToListAsync();
        return Ok(parts);
    }

    // POST: api/parts
    [HttpPost]
    public async Task<ActionResult<Part>> PostPart(Part part)
    {
        // Проверяем, существует ли машина с указанным ID
        var carExists = await _context.Cars.AnyAsync(c => c.Id == part.CarId);
        if (!carExists)
        {
            return BadRequest("Car not found");
        }

        _context.Parts.Add(part);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPartsByCar", new { carId = part.CarId }, part);
    }

    // PUT: api/parts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPart(int id, Part part)
    {
        if (id != part.Id)
        {
            return BadRequest();
        }

        _context.Entry(part).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PartExists(id))
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

    // DELETE: api/parts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePart(int id)
    {
        var part = await _context.Parts.FindAsync(id);
        if (part == null)
        {
            return NotFound();
        }

        _context.Parts.Remove(part);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PartExists(int id)
    {
        return _context.Parts.Any(e => e.Id == id);
    }
}
