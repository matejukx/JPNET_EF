namespace JPNET_EF.Controllers;

using Abstractions.dbo;
using Abstractions.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;

[Route("/api/orders")]
public class OrderController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public OrderController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        var orders = _context.Orders
            .Include(o => o.Client)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Item)
            .ToList();
            return Ok(orders);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OrderResource orderResource)
    {
       
        try
        {
            var order = _mapper.Map<Order>(orderResource);
            await _context.Orders.AddAsync(order);

            var orderItems = 
                orderResource.Items
                    .GroupBy(i => i.Id)
                    .Select(g =>
                new OrderedItem
                {
                    ItemId = g.First().Id,
                    Quantity = g.Count(),
                    OrderId = order.Id
                }).ToList();
            foreach (var orderedItem in orderItems)
            {
                await _context.OrderedItems.AddAsync(orderedItem);
            }
            
            await _context.SaveChangesAsync();
            return Ok(order);
        }
        catch (Exception ex)
        {
            return StatusCode(503, ex.Message);
        }
       
    }
    
}