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
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace DoAnASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private readonly DpContext _context;

        public AccountsController(DpContext context)
        {
            _context = context;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.ListAccount = _context.Account.ToList();
            base.OnActionExecuted(context);
        }
        // GET: Admin/Accounts
        public async Task<IActionResult> Index(string? id)
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                return Redirect("/Admin/Login");
            }
            JObject us = JObject.Parse(HttpContext.Session.GetString("admin"));
            ViewBag.Name = us.SelectToken("Name").ToString();
            Account account = null;
            if (id != null)
            {
                account = await _context.Account.FirstOrDefaultAsync(m => m.Email == id);
            }
            return View(account);
        }

        // POST: Admin/Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Name,Password,Status,Role,PhoneNumber,Address")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                account.Password = StringProcessing.CreateMD5Hash(account.Password);
                _context.Update(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // POST: Admin/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Email,Name,Password,Status,Role,PhoneNumber,Address")] Account account)
        {
            if (id != account.Email)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    account.Password = StringProcessing.CreateMD5Hash(account.Password);
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Email))
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
            return View(account);
        }
        private bool AccountModelExists(string email)
        {
            throw new NotImplementedException();
        }
        private bool AccountExists(string id)
        {
            return _context.Account.Any(e => e.Email == id);
        }
    }
}
