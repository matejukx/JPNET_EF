using Microsoft.AspNetCore.Mvc;

namespace JPNET_EF.Controllers;

using Abstractions.dbo;
using Abstractions.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Repository;

[Route("api/clients")]
[EnableCors]
public class ClientController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public ClientController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
        return await _context.Clients.ToListAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
    
        if (client == null)
        {
            return NotFound();
        }
    
        return client;
    }
    
    [HttpGet("name/{name}")]
    public async Task<ActionResult<IEnumerable<Client>>> GetClientsByName([FromRoute] string name)
    {
        var clients = await _context.Clients.Where(c => c.Name.Contains(name)).ToListAsync();
    
        if (clients == null)
        {
            return NotFound();
        }
    
        return clients;
    }

    [HttpGet("setMain/{id}")]
    public async Task<ActionResult<Client>> SetMain(int id)
    {
        var client = await _context.Clients.FindAsync(id);
    
        if (client == null)
        {
            return NotFound();
        }
        
        this.HttpContext.Session.SetInt32("mainId", id);
    
        return client;
    }
    
    [HttpGet("main")]
    public async Task<ActionResult<Client>> GetMain()
    {
        var id = this.HttpContext.Session.GetInt32("mainId");
        if (id == null)
        {
            return NotFound();
        }
        
        var client = await _context.Clients.FindAsync(id);
    
        if (client == null)
        {
            return NotFound();
        }
    
        return client;
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutClient(int id, Client client)
    {
        if (id != client.Id)
        {
            return BadRequest();
        }
    
        _context.Entry(client).State = EntityState.Modified;
    
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClientExists(id))
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
    
    [HttpPost]
    public async Task<ActionResult<Client>> PostClient([FromBody] ClientResource clientResource)
    {
        if (string.IsNullOrEmpty(clientResource.IpAddress))
        {
            var client = _mapper.Map<Client>(clientResource);
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }
        else
        {
            var client = _mapper.Map<InternetClient>(clientResource);
            _context.InternetClients.Add(client);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<Client>> DeleteClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound();
        }
    
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    
        return client;
    }

    private bool ClientExists(int id)
    {
       return _context.Clients.Any(c => c.Id == id);
    }
}