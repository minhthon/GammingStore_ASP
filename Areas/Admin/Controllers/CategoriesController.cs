using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAnASP.Areas.Admin.Models;
using DoAnASP.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace DoAnASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly DpContext _context;

        public CategoriesController(DpContext context)
        {
            _context = context;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.ListCategory = _context.Category.ToList();
            base.OnActionExecuted(context);
        }
        // GET: Admin/Category
        public async Task<IActionResult> Index(int? id)
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                return Redirect("/Admin/Login");
            }
            JObject us = JObject.Parse(HttpContext.Session.GetString("admin"));
            ViewBag.Name = us.SelectToken("Name").ToString();
            Category category = null;
            if (id != null)
            {
                category = await _context.Category.FirstOrDefaultAsync(m => m.IDCategory == id);
            }
            return View(category);
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDCategory,CategoryName,Image,Status")] Category category, IFormFile ful)
        {
            if (ModelState.IsValid)
            {
                //Them vao context
                _context.Add(category);
                //Luu vao database
                await _context.SaveChangesAsync();
                // Luu duong dan hinh anh
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/ImageCategory",
                category.IDCategory + "." + ful.FileName.Split(".")[ful.FileName.Split(".").Length - 1]);                 // Luu file
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ful.CopyToAsync(stream);
                }
                category.Image = category.IDCategory + "." + ful.FileName.Split(".")[ful.FileName.Split(".").Length - 1];
                // categoryModel.Img = ful.FileName;
                // Cap nhat san pham vo context
                _context.Update(category);
                // Cap nhat vao database
                 await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }



        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDCategory,CategoryName,Image,Status")] Category category,IFormFile ful)
        {
            if (id != category.IDCategory)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem người dùng có chọn ảnh mới để chỉnh sửa hay không
                    var path = "";
                    if (ful != null)
                    {
                        if (category.Image != null)
                        {                                                      // Xóa hình cũ
                                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/ImageCategory", category.Image);
                                System.IO.File.Delete(path);
                           
                            
                        }
                        else
                        {                            // Lưu đường dẫn hình mới
                            path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/ImageCategory",
                            category.IDCategory + "." + ful.FileName.Split(".")[ful.FileName.Split(".").Length - 1]);
                        }
                        // Lưu hình ảnh
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await ful.CopyToAsync(stream);
                        }
                        // Cập nhật trường dữ liệu
                        category.Image = category.IDCategory + "." + ful.FileName.Split(".")[ful.FileName.Split(".").Length - 1];
                    }
                    // Cập nhật sản phẩm database và context 
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryModelExists(category.IDCategory))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        private bool CategoryModelExists(int iDCategory)
        {
            throw new NotImplementedException();
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.IDCategory == id);
        }
    }
}