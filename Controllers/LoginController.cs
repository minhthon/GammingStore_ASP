using DoAnASP.Areas.Admin.Models;
using DoAnASP.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnASP.Controllers
{
    public class LoginController : Controller
    {
        private readonly DpContext _context;

        public LoginController(DpContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.ListCategory = _context.Category.ToList(); // danh sách product
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email,Password")] Account acc)
        {
            ViewBag.ListCategory = _context.Category.ToList(); // danh sách product

            var r = _context.Account.Where(m => (m.Email == acc.Email && m.Status == true &&  m.Password == StringProcessing.CreateMD5Hash(acc.Password))).ToList();
            if (r.Count == 0)
            {
                return View("Index");
            }
            foreach (var s in r)
            {
                acc.Name = s.Name;
                acc.Email = s.Email;
                acc.Password = s.Password;
                acc.Role = s.Role;
                acc.Address = s.Address;
                acc.PhoneNumber = s.PhoneNumber;
                acc.Status = s.Status;

            }
            var str = JsonConvert.SerializeObject(acc);
            HttpContext.Session.SetString("user", str);
            if (r[0].Role.Equals("User"))
            {
                return Redirect("/Home");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Name,Password,Status,Role,PhoneNumber,Address")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                account.Password = StringProcessing.CreateMD5Hash(account.Password);
                account.Role = "User";
                account.Status = true;
                var str = JsonConvert.SerializeObject(account);
                HttpContext.Session.SetString("user", str);
                _context.Update(account);
                await _context.SaveChangesAsync();
            }
            return Redirect("/Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

    }
}
