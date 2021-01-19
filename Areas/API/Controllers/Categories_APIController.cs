using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnASP.Areas.Admin.Models;
using DoAnASP.Data;

namespace DoAnASP.Areas.Admin.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Categories_APIController : ControllerBase
    {
        private readonly DpContext _context;

        public Categories_APIController(DpContext context)
        {
            _context = context;
        }

        // GET: api/Categories_API
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            return await _context.Category.ToListAsync();
        }

        // GET: api/Categories_API/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }
       

        public class CategoryUpload
        {
            public int IDCategory { get; set; }
            public bool Status { get; set; }
        }

        [HttpPost]
        public string UpdateStatus(CategoryUpload category)
        {
            (from p in _context.Category
             where p.IDCategory == category.IDCategory
             select p).ToList().ForEach(x => x.Status = category.Status);
            _context.SaveChanges();
            return "{\"id\":" + category.IDCategory + ",\"stt\":\"" + category.Status + "\"}";
        }
        // PUT: api/Categories_API/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutCategory(int id, Category category)
        {
            if (id != category.IDCategory)
            {   
                return "0";
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();          
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return "0";
                }
                else
                {
                    throw;
                }
            }

            return "{\"id\":"+id+",\"stt\":\""+category.Status+"\"}";
        }

        // POST: api/Categories_API
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.IDCategory }, category);
        }

        // DELETE: api/Categories_API/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return "0";
            }
          
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return "Success";
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.IDCategory == id);
        }
    }
}
