using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnASP.Areas.Admin.Models;
using DoAnASP.Data;

namespace DoAnASP.Areas.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Product_APIController : ControllerBase
    {
        private readonly DpContext _context;

        public Product_APIController(DpContext context)
        {
            _context = context;
        }

        // GET: api/Product_API
       
        // GET: api/Product_API/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        public class ProductUpdate
        {
            public int IDProduct { get; set; }
            public int Status { get; set; }
        }

        [HttpPost]
        public string UpdateStatus(ProductUpdate product)
        {
            (from p in _context.Product
             where p.IDProduct == product.IDProduct
             select p).ToList().ForEach(x => x.Status = product.Status);
            _context.SaveChanges();
            return "{\"id\":" + product.IDProduct + ",\"stt\":\"" + product.Status + "\"}";
        }

        // PUT: api/Product_API/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.IDProduct)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Product_API
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.IDProduct }, product);
        }

        // DELETE: api/Product_API/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return "0";
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return "Success";
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.IDProduct == id);
        }
    }
}
