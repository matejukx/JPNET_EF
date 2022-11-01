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
        // if any of the orders client is referenceable as InternetClient then replace it with InternetClient
        foreach (var order in orders)
        {
            if (!string.IsNullOrEmpty((order as InternetOrder)?.IpAddress))
            {
                order.IsInternet = true;
            }
        }
        return Ok(orders);
    }
    
    [HttpPost("/finish/{id}")]
    public IActionResult Finish(int id)
    {
        var order = _context.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Item)
            .FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }

        foreach (var orderItem in order.Items)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == orderItem.ItemId);
            if (item == null)
            {
                return NotFound();
            }
            item.Quantity -= orderItem.Quantity;
            if (item.Quantity < 0)
            {
                return BadRequest("Not enough items in stock");
            }
        }

        order.IsFinished = true;
        _context.SaveChanges();
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OrderResource orderResource)
    {
       
        try
        {
            if (!orderResource.IsInternet) return await AddOrder(orderResource);
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            return await AddInternetOrder(orderResource, ipAddress);

        }
        catch (Exception ex)
        {
            return StatusCode(503, ex.Message);
        }
       
    }

    private async Task<ObjectResult> AddOrder(OrderResource orderResource)
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
                        Quantity = 0,
                        OrderId = order.Id
                    }).ToList();
        foreach (var orderedItem in orderItems)
        {
            orderedItem.Quantity = orderResource.Items.Count(i => i.Id == orderedItem.ItemId);
            await _context.OrderedItems.AddAsync(orderedItem);
        }
            
        await _context.SaveChangesAsync();
        return Ok(order);
    }
    
    private async Task<ObjectResult> AddInternetOrder(OrderResource orderResource, string IpAddress)
    {
        var order = _mapper.Map<InternetOrder>(orderResource);
        order.IpAddress = IpAddress;
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
    
}