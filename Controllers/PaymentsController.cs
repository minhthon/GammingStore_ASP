using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAnASP.Areas.Admin.Models;
using DoAnASP.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;

namespace DoAnASP.Areas
{
    
    public class PaymentsController : Controller
    {
        private readonly DpContext _context;

        public PaymentsController(DpContext context)
        {
            _context = context;
        }
       
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.ListCategory = _context.Category.ToList(); // danh sách product
            ViewBag.ListProduct = _context.Product.ToList();
           
            base.OnActionExecuted(context);
        }
        public const string CARTKEY = "cart";
   
        // GET: Admin/Payments
        public async Task<IActionResult>Index()
        {

            if (HttpContext.Session.GetString("admin") == null)
            {
                return Redirect("/Admin/Login");
            }
            JObject us = JObject.Parse(HttpContext.Session.GetString("admin"));
            ViewBag.Name = us.SelectToken("Name").ToString();
              
            string email = us.SelectToken("Email").ToString();
            var payment = from p in _context.Payment where p.IDUser == email select p;
            ViewBag.Payment = payment;
          
            return View();
          
        }

        // GET: Admin/Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .FirstOrDefaultAsync(m => m.IDPayment == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Admin/Payments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDPayment,IDUser,PaymentMethod,ShippingUnit,Note,Address,Cart,Status")] Payment payment)
        {
            if (HttpContext.Session.GetString("user") == null)
            {
                return Redirect("/Login");
            }
            JObject us = JObject.Parse(HttpContext.Session.GetString("user"));
            ViewBag.Name = us.SelectToken("Name").ToString();
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (ModelState.IsValid)
            {
                _context.Add(payment);             
                await _context.SaveChangesAsync();
                payment.Cart = jsoncart;

                _context.Update(payment);
                await _context.SaveChangesAsync();
                session.Clear();
                return Redirect("/Product_Cart");
            }
            return Redirect("/");
        }

        // GET: Admin/Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

               
        // POST: Admin/Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDPayment,IDUser,PaymentMethod,ShippingUnit,Note,Address,Cart")] Payment payment)
        {
            if (id != payment.IDPayment)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.IDPayment))
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
            return View(payment);
        }

        // GET: Admin/Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .FirstOrDefaultAsync(m => m.IDPayment == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Admin/Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payment.FindAsync(id);
            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.IDPayment == id);
        }
    }
}
