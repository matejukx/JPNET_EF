namespace JPNET_EF.Controllers;

using Abstractions.Entity;
using Microsoft.AspNetCore.Mvc;
using Repository;

[Route("api/items")]
public class ItemController : ControllerBase
{
    private ApplicationDbContext _context;
    
    public ItemController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Items.ToList());
    }
    
    [HttpPost]
    public IActionResult Post([FromBody] Item item)
    {
        try
        {
            _context.Items.Add(item);
            _context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(503, ex.Message);
        }
    }
    
    [HttpPut("/add/{id}")]
    public IActionResult Put([FromQuery] int quanity, [FromRoute] int id)
    {
        try
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();
            item.Quantity = quanity;
            _context.Items.Update(item);
            _context.SaveChanges();
            return Ok();

        }
        catch (Exception ex)
        {
            return StatusCode(503, ex.Message);
        }
    }
}