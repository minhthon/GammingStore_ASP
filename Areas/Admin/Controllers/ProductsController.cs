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
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace DoAnASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly DpContext _context;

        public ProductsController(DpContext context)
        {
            _context = context;
        }

    
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.ListCategory = _context.Category.ToList(); // danh sách product
            ViewBag.ListProduct = _context.Product.ToList();
            base.OnActionExecuted(context);
        }
        // GET: Admin/Products
        public async Task<IActionResult> Index(int? id)
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                return Redirect("/Admin/Login");
            }
            JObject us = JObject.Parse(HttpContext.Session.GetString("admin"));
            ViewBag.Name = us.SelectToken("Name").ToString();
            Product product = null;
            if (id != null)
            {
                product = await _context.Product.FirstOrDefaultAsync(m => m.IDProduct == id);
            }
            return View(product);
        }

        // GET: Admin/Products/Details/5
     
        // GET: Admin/Products/Create
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDProduct,ProductName,Price,Image,Classify,Description,Status,Quantity,Producer,Origin,WarrantyPeriod,CPU,Ram,VGA,Hard_drive,Display,Connector,Audio,Wifi,Bluetooth,OperatingSystem,Battery,Weight,Color,Size")] Product product,IFormFile[] ful)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();

                for (int i = 0; i < ful.Length; i++)
                {

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/ImageProduct",
                    "Laptop" + (product.IDProduct) +(i)+ "." + ful[i].FileName.Split(".")[ful[i].FileName.Split(".").Length - 1]);                 // Luu file

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await ful[i].CopyToAsync(stream);
                    }
                    product.Image += "Laptop" + (product.IDProduct) +i + "." + ful[i].FileName.Split(".")[ful[i].FileName.Split(".").Length - 1] + " ";
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }

            return View(product);       
        }

      

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDProduct,ProductName,Price,Image,Classify,Description,Status,Quantity,Producer,Origin,WarrantyPeriod,CPU,Ram,VGA,Hard_drive,Display,Connector,Audio,Wifi,Bluetooth,OperatingSystem,Battery,Weight,Color,Size")] Product product,IFormFile[] ful)
        {
            if (id != product.IDProduct)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string[] img;

                try
                {
                    // Kiểm tra xem người dùng có chọn ảnh mới để chỉnh sửa hay không
                    if (ful.Count() > 0)
                    {
                        //Xóa hình cũ
                        if (product.Image != null)
                        {
                            img = product.Image.Split(" ");
                            for (int i = 0; i < img.Length; i++)
                            {
                                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/ImageProduct/" + img[i]);
                                System.IO.File.Delete(path);
                            }
                        }
                        product.Image = "";
                        for (int i = 0; i < ful.Count(); i++)
                        {

                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/ImageProduct",
                              "Laptop" + (product.IDProduct) + i + "." + ful[i].FileName.Split(".")[ful[i].FileName.Split(".").Length - 1]);                 // Luu file

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await ful[i].CopyToAsync(stream);
                            }
                            product.Image += "Laptop" + (product.IDProduct) + i + "." + ful[i].FileName.Split(".")[ful[i].FileName.Split(".").Length - 1] + " ";
                            _context.Update(product);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        _context.Update(product);
                        await _context.SaveChangesAsync();

                    }

                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductModelExists(product.IDProduct))
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


            return View(product);
        }

        private bool ProductModelExists(int iDProduct)
        {
            throw new NotImplementedException();
        }

        
        // GET: Admin/Products/Delete/5
      
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.IDProduct == id);
        }
    }
}
