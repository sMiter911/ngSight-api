using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Advantage.API.Models;

namespace Advantage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApiContext _context;

        public OrdersController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // Get api/order/pagenNumber/pageSize
        [HttpGet("{pageIndex:int}/{pageSize:int}")]
        public IActionResult Get(int pageIndex, int pageSize)
        {
            var data = _context.Orders.Include(o => o.Customer).OrderByDescending(c => c.Placed);
            var page = new PaginatedResponse<Order>(data, pageIndex, pageSize);
            var totalCount = data.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);
            var response = new
            {
                Page = page,
                TotalPages = totalPages
            };

            return Ok(response);
        }

        [HttpGet("ByProvince")]
        public IActionResult ByProvince()
        {
            var orders = _context.Orders.Include(o => o.Customer).ToList();
            var groupedResult = orders.GroupBy(o => o.Customer.Province).ToList().Select(grp => new
            {
                Province = grp.Key,
                Total = grp.Sum(x => x.Total)

            }).OrderByDescending(res => res.Total).ToList();

            return Ok(groupedResult);
        }

        [HttpGet("ByCustomer/{n}")]
        public IActionResult ByCustomer(int n)
        {
            var orders = _context.Orders.Include(o => o.Customer).ToList();
            var groupedResult = orders.GroupBy(o => o.Customer.Id).ToList().Select(grp => new
            {
                _context.Customers.Find(grp.Key).Name,
                Total = grp.Sum(x => x.Total)

            }).OrderByDescending(res => res.Total).Take(n).ToList();

            return Ok(groupedResult);
        }



        // GET: api/Orders/5
        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = _context.Orders.Include(o => o.Customer).First(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
