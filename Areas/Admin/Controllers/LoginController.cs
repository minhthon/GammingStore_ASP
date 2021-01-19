using DoAnASP.Areas.Admin.Models;
using DoAnASP.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly DpContext _context;
        public LoginController(DpContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email,Password")] Account acc)
        {
            var r = _context.Account.Where(m => (m.Email == acc.Email &&m.Status==true && m.Password == StringProcessing.CreateMD5Hash(acc.Password))).ToList();
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
            HttpContext.Session.SetString("admin", str);
            if (r[0].Role.Equals("Admin"))
            {
                return Redirect("/Admin/Home");
            }
            return RedirectToAction("Index");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
    }
}
